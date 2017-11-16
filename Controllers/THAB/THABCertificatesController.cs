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
        private readonly IReader<Person> _peopleService;
        private readonly IReader<ThabCertificate> _thabCertificateService;
        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABCertificatesController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<ThabCertificate> thabCertificateService,
                                         IReader<Person> peopleService,
                                         IMapper mapper)
        {
            _peopleService = peopleService;
            _thabCertificateService = thabCertificateService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber, [FromQuery]string insz)
        {
            var certificates = await _thabCertificateService.SearchAsync(new ThabCertificateSearch() { SiNumber = sinumber, Insz = insz });

            return Ok(certificates);

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