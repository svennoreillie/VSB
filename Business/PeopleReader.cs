using System.Collections.Generic;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Search;
using VSBaseAngular.Models.Keys;
using System.Threading.Tasks;
using System.ServiceModel;
using System;
using Microsoft.Extensions.Options;
using VSBaseAngular.Helpers.Options;
using System.Linq;
using AutoMapper;
using System.Net.Http;
using System.Web;
using System.IO;
using ZvzService;
using BobService;
using ThabService;

namespace VSBaseAngular.Business
{
    public class PeopleReader : IReader<Models.Person>
    {
        private readonly IOptions<AppConfig> _config;
        private readonly IOptions<ApiConfig> _apiConfigs;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private IZvzService _zvzService;
        private readonly IBobService _bobService;
        private IThabService _thabService;

        public PeopleReader(IOptions<AppConfig> config,
            IOptions<ApiConfig> apiConfigs,
            IServiceFactory<IZvzService> zvzServiceFactory,
            IServiceFactory<IBobService> bobServiceFactory,
            IServiceFactory<IThabService> thabServiceFactory,
            IMapper mapper,
            HttpClient client)
        {
            _config = config;
            _apiConfigs = apiConfigs;
            _mapper = mapper;
            _client = client;

            _zvzService = zvzServiceFactory.GetService();
            _bobService = bobServiceFactory.GetService();
            _thabService = thabServiceFactory.GetService();
        }


        public async Task<Models.Person> GetAsync(KeySet<Models.Person> key)
        {
            var pKey = key as PersonKey;
            return (await SearchBySiNumber(pKey.SiNumber)).FirstOrDefault();
        }

        public async Task<IEnumerable<Models.Person>> SearchAsync(SearchBaseModel<Models.Person> model)
        {
            PersonSearch search = model as PersonSearch;
            if (search == null) throw new ArgumentException($"{nameof(model)} is not a PersonSearch object");

            List<Task<IEnumerable<Models.Person>>> peopleTasks = new List<Task<IEnumerable<Models.Person>>>();

            if (!string.IsNullOrEmpty(search.Insz)) peopleTasks.Add(SearchByInsz(search));
            if (!string.IsNullOrEmpty(search.MemberNr)) peopleTasks.Add(SearchByMemberNr(search));
            if (search.SiNumber.HasValue) peopleTasks.Add(SearchBySiNumber(search.SiNumber.Value));
            if (!string.IsNullOrEmpty(search.Name)) peopleTasks.Add(SearchByName(search));
            switch (search.Pillar)
            {
                case "ZVZ":
                    peopleTasks.Add(SearchZVZState(search));
                    break;
                case "BOB":
                    peopleTasks.Add(SearchBOBState(search));
                break;
                case "THAB":
                    peopleTasks.Add(SearchTHABState(search));
                    break;
                default:
                    break;
            }

            var peopleLists = await Task.WhenAll(peopleTasks);
            var people = peopleLists.SelectMany(p => p);

            //temp fix for federation state search => should be in calls for mainframe!!!!!!
            if (search.Federation.HasValue && search.Federation.Value % 100 != 0)
            {
                people = people.Where(p => p.FederationNumber == search.Federation.Value);
            }

            return people;
        }


        private async Task<IEnumerable<Models.Person>> SearchZVZState(PersonSearch model)
        {
            var request = new GetPersonsByWarrantiesStateRequest
            {
                Accepted = model.StateCompleted ?? false,
                Pending = model.StateInitiated ?? false,
                Refused = model.StateRejected ?? false,
                ReferenceDecisionMonth = GetFirstOfMonth(model.StateRejectedDate),
                ReferenceStartMonth = GetFirstOfMonth(model.StateCompletedDate)
            };
            var warranties = await _zvzService.GetPersonsByWarrantiesStateAsync(request);
            if (warranties.BusinessMessages.Any()) throw new Exception(warranties.BusinessMessages.FirstOrDefault().MessageString);

            return await GetSiNumbers(model, warranties?.Value?.Sinrs?.ToList());
        }

        private async Task<IEnumerable<Models.Person>> SearchBOBState(PersonSearch model)
        {
            var request = new BobService.GetPersonsByCertificateStateRequest
            {
                Accepted = model.StateCompleted ?? false,
                Pending = model.StateInitiated ?? false,
                Refused = model.StateRejected ?? false,
                ReferenceDecisionMonth = GetFirstOfMonth(model.StateRejectedDate),
                ReferenceStartMonth = GetFirstOfMonth(model.StateCompletedDate)
            };
            var certificates = await _bobService.GetPersonsByCertificateStateAsync(request);
            if (certificates.BusinessMessages.Any()) throw new Exception(certificates.BusinessMessages.FirstOrDefault().MessageString);

            return await GetSiNumbers(model, certificates?.Value?.Sinrs?.ToList());
        }

        private async Task<IEnumerable<Models.Person>> SearchTHABState(PersonSearch model)
        {
            var request = new ThabService.GetPersonsByCertificateStateRequest
            {
                Accepted = model.StateCompleted ?? false,
                Pending = model.StateInitiated ?? false,
                Refused = model.StateRejected ?? false,
                ReferenceDecisionMonth = GetFirstOfMonth(model.StateRejectedDate),
                ReferenceStartMonth = GetFirstOfMonth(model.StateCompletedDate)
            };
            var certificates = await _thabService.GetPersonsByCertificateStateAsync(request);
            if (certificates.BusinessMessages.Any()) throw new Exception(certificates.BusinessMessages.FirstOrDefault().MessageString);

            return await GetSiNumbers(model, certificates?.Value?.Sinrs?.ToList());
        }

        private static DateTime GetFirstOfMonth(DateTime? date)
        {
            if (!date.HasValue) date = DateTime.Now;
            DateTime d = date.Value;
            return new DateTime(d.Year, d.Month, 1);
        }

        private async Task<IEnumerable<Models.Person>> SearchByName(PersonSearch model)
        {
            var uriBuilder = new UriBuilder(CreateUrl("members/name"));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["federation"] = _config.Value?.Environment.ToString();
            query["firstname"] = model.FirstName;
            query["name"] = model.Name;
            query["skip"] = model.Skip.ToString();
            query["limit"] = model.Limit.ToString();
            uriBuilder.Query = query.ToString();

            var response = await _client.GetAsync(uriBuilder.ToString());

            return response.ContentAsType<IEnumerable<Models.Person>>();
        }



        private async Task<IEnumerable<Models.Person>> SearchByMemberNr(PersonSearch model)
        {
            var uriBuilder = new UriBuilder(CreateUrl("members/membernr"));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["federation"] = model.Federation.ToString();
            query["membernr"] = model.MemberNr;
            uriBuilder.Query = query.ToString();

            var response = await _client.GetAsync(uriBuilder.ToString());

            return new List<Models.Person> { response.ContentAsType<Models.Person>() };
        }

        private async Task<IEnumerable<Models.Person>> SearchBySiNumber(long siNumber)
        {
            var response = await _client.GetAsync(CreateUrl($"members/sinumber/{siNumber}"));

            return new List<Models.Person> { response.ContentAsType<Models.Person>() };
        }

        private async Task<IEnumerable<Models.Person>> SearchBySiNumbers(IEnumerable<long> siNumbers)
        {
            var response = await _client.PostAsync(CreateUrl($"members/sinumber"), new JsonContent(new { siNumbers = siNumbers}));

            return response.ContentAsType<IEnumerable<Models.Person>>();
        }

        private async Task<IEnumerable<Models.Person>> SearchByInsz(PersonSearch model)
        {
            var response = await _client.GetAsync(CreateUrl($"members/insz/{model.Insz}"));

            return new List<Models.Person> { response.ContentAsType<Models.Person>() };
        }


        private string CreateUrl(string endpoint)
        {
            var section = _apiConfigs.Value?.Configs?.FirstOrDefault(a => a.ApplicationName == "Popu");
            if (section == null) throw new Exception("No configuration found for Popu Api");

            return Path.Combine(section.Url, endpoint);
        }

        private async Task<IEnumerable<Models.Person>> GetSiNumbers(PersonSearch model, List<long> sinumbers)
        {
            if (model.Limit > 0) sinumbers = sinumbers.Skip(model.Skip).Take(model.Limit).ToList();

            //  !!!   Commented code is faster => but popu doesn't like it so we bulk search instead
            // List<Task<Models.Person>> tasks = new List<Task<Models.Person>>();
            // foreach (var snr in sinumbers)
            // {
            //     tasks.Add(this.SearchBySiNumber(snr));
            // }
            // return await Task.WhenAll(tasks);

            return await this.SearchBySiNumbers(sinumbers);
        }
    }
}