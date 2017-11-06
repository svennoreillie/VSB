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

namespace VSBaseAngular.Business
{
    public class PeopleReader : IReader<Person>
    {
        private readonly IOptions<AppConfig> _config;
        private readonly IOptions<ApiConfig> _apiConfigs;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;

        public PeopleReader(IOptions<AppConfig> config,
            IOptions<ApiConfig> apiConfigs,
            IMapper mapper,
            HttpClient client)
        {
            _config = config;
            _apiConfigs = apiConfigs;
            _mapper = mapper;
            _client = client;
        }


        public async Task<Person> GetAsync(KeySet<Person> key)
        {
            var pKey = key as PersonKey;
            return await SearchBySiNumber(pKey.SiNumber);
        }

        public async Task<IEnumerable<Person>> SearchAsync(SearchBaseModel<Person> model)
        {
            PersonSearch search = model as PersonSearch;
            if (search == null) throw new ArgumentException($"{nameof(model)} is not a PersonSearch object");

            List<Person> people = new List<Person>();

            if (!string.IsNullOrEmpty(search.Insz)) people.Add(await SearchByInsz(search));
            if (!string.IsNullOrEmpty(search.MemberNr)) people.Add(await SearchByMemberNr(search));
            if (search.SiNumber > 0) people.Add(await SearchBySiNumber(search.SiNumber));
            if (!string.IsNullOrEmpty(search.Name)) people.AddRange(await SearchByName(search));

            return people;
        }



        private async Task<IEnumerable<Person>> SearchByName(PersonSearch model)
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

            return response.ContentAsType<IEnumerable<Person>>();
        }

        

        private async Task<Person> SearchByMemberNr(PersonSearch model)
        {
            var uriBuilder = new UriBuilder(CreateUrl("members/membernr"));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["federation"] = model.Federation.ToString();
            query["membernr"] = model.MemberNr;
            uriBuilder.Query = query.ToString();

            var response = await _client.GetAsync(uriBuilder.ToString());

            return response.ContentAsType<Person>();
        }

        private async Task<Person> SearchBySiNumber(long siNumber)
        {
            var response = await _client.GetAsync(CreateUrl($"member/sinumber/{siNumber}"));

            return response.ContentAsType<Person>();
        }

        private async Task<Person> SearchByInsz(PersonSearch model)
        {
            var response = await _client.GetAsync(CreateUrl($"member/insz/{model.Insz}"));

            return response.ContentAsType<Person>();
        }


        private string CreateUrl(string endpoint)
        {
            var section = _apiConfigs.Value?.Configs?.FirstOrDefault(a => a.ApplicationName == "Popu");
            if (section == null) throw new Exception("No configuration found for Popu Api");

            return Path.Combine(section.Url, endpoint);
        }
    }
}