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
using ThabService;

namespace VSBaseAngular.Business
{
    public class ThabCertificateReader : IReader<ThabCertificate>
    {
        private readonly IThabService _service;
        private IReader<Person> _peopleService;
        private readonly IMapper _mapper;

        public ThabCertificateReader(IServiceFactory<IThabService> thabServiceFactory,
            IReader<Person> peopleService,
            IMapper mapper)
        {
            _service = thabServiceFactory.GetService();
            _peopleService = peopleService;
            _mapper = mapper;
        }


        public async Task<ThabCertificate> GetAsync(KeySet<ThabCertificate> key)
        {
            var thabKey = key as ThabCertificateKey;
            if (thabKey == null) throw new ArgumentException("key not of ThabCertificateKey type");
            var certificates = await this.GetCertificates(thabKey.SiNumber, thabKey.Insz);
            return certificates.FirstOrDefault(c => c.CertificateId == thabKey.CertificateId);
        }

        public async Task<IEnumerable<ThabCertificate>> SearchAsync(SearchBaseModel<ThabCertificate> model)
        {
            ThabCertificateSearch search = model as ThabCertificateSearch;
            if (search == null) throw new ArgumentException($"{nameof(model)} is not a ThabCertificateSearch object");

            var certificates = await this.GetCertificates(search.SiNumber, search.Insz);
            
            if (search.Skip > 0) certificates = certificates.Skip(search.Limit);
            if (search.Limit > 0) certificates = certificates.Take(search.Limit);

            return certificates;
        }

        private async Task<IEnumerable<ThabCertificate>> GetCertificates(long sinumber, string insz) {
            if (string.IsNullOrEmpty(insz)) {
                var person = await _peopleService.GetAsync(new PersonKey(sinumber));
                insz = person?.Insz;
            }

            var request = new GetCertificatesRequest()
            {
                SiNumber = sinumber,
                Insz = insz
            };

            var response = await _service.GetCertificatesAsync(request);
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                throw new Exception(string.Join(';', response.BusinessMessages.Select(bm => bm.MessageString)));

            return _mapper.Map<IEnumerable<ThabCertificate>>(response?.Value?.Certificates);
        }
    }
}