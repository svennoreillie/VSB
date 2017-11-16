using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ThabService;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;
using System;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABPaymentsController : BaseController
    {
        private readonly IReader<ThabCertificate> _thabcertificateService;
        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABPaymentsController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<ThabCertificate> thabcertificateService,
                                         IMapper mapper)
        {
            _thabcertificateService = thabcertificateService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/payments/{referenceDate:datetime}")]
        public async Task<IActionResult> Get(long sinumber, DateTime referenceDate)
        {
            var request = new GetPaymentsRequest()
            {
                SiNumber = sinumber,
                ReferenceDate = referenceDate
            };
            var response = await _service.GetPaymentsAsync(request);
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var mappedModels = _mapper.Map<IEnumerable<ThabPayment>>(response?.Value?.Payments);
            return Ok(mappedModels);
        }

        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/payments")]
        public async Task<IActionResult> Get(long sinumber, [FromQuery]string insz)
        {
            var certificates = await _thabcertificateService.SearchAsync(new ThabCertificateSearch { SiNumber = sinumber, Insz = insz });

            List<ThabPayment> payments = new List<ThabPayment>();

            foreach (var cert in certificates)
            {
                var request = new GetPaymentsRequest()
                {
                    SiNumber = sinumber,
                    ReferenceDate = cert.ReferenceDate
                };
                var response = await _service.GetPaymentsAsync(request);
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);

                var mappedModels = _mapper.Map<IEnumerable<ThabPayment>>(response?.Value?.Payments);
                payments.AddRange(mappedModels);
            }

            return Ok(payments);
        }
    }
}
