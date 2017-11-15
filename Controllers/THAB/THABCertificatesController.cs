using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ThabService;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABCertificatesController : BaseController
    {
        public IReader<Person> _peopleService { get; }

        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABCertificatesController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<Person> peopleService,
                                         IMapper mapper)
        {
            _peopleService = peopleService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber, [FromQuery]string insz)
        {
            if (string.IsNullOrEmpty(insz))
            {
                var person = await _peopleService.GetAsync(new PersonKey(sinumber));
                if (person == null) return BadRequest("INSZ could not be found");
                insz = person.Insz;
            }

            var request = new GW.VSB.THAB.Contracts.GetCertificates.GetCertificatesRequest()
            {
                SiNumber = sinumber,
                Insz = insz
            };

            var response = _service.GetCertificates(request);
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var mappedModels = _mapper.Map<IEnumerable<ThabCertificate>>(response?.Value?.Certificates);
            return Ok(mappedModels);

            // List<ThabCertificate> certificates = new List<ThabCertificate>();

            // foreach (var certificate in response?.Value?.Certificates)
            // {
            //     var remarkRequest = new GW.VSB.THAB.Contracts.GetRemark.GetRemarkRequest()
            //     {
            //         SiNumber = sinumber,
            //         ReferenceDate = certificate.InitialDate
            //     };
            //     var remarkResponse = _service.GetRemark(remarkRequest);
            //     if (remarkResponse.BusinessMessages != null && remarkResponse.BusinessMessages.Length > 0)
            //         return BadRequest(remarkResponse.BusinessMessages);

            //     var cert = _mapper.Map<ThabCertificate>(certificate);
            //     cert.Remark = remarkResponse?.Value?.Remark;
            //     certificates.Add(cert);
            // }

            // return Ok(certificates);
        }
    }
}