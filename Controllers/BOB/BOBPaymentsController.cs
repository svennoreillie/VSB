using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BobService;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class BOBPaymentsController : BaseController
    {
        private readonly IBobService _service;
        private IMapper _mapper;

        public BOBPaymentsController(IServiceFactory<IBobService> bobServiceFactory, IMapper mapper)
        {
            _service = bobServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber}/certificates/{certificateId}/payments")]
        public async Task<IActionResult> GetSpecific(long sinumber, string certificateId)
        {
            var certificates = await _service.GetCertificatesAsync(new GetCertificatesRequest { SiNumber = sinumber });
            var certificate = certificates?.Value?.Certificates?.FirstOrDefault(c => c.Id == certificateId);


            var response = await _service.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = sinumber, ReferenceDate = certificate.From });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);
            var mappedPayments = _mapper.Map<IEnumerable<BobPayment>>(response.Value?.Payments, opt => opt.Items["Id"] = certificateId);
            return Ok(mappedPayments);
        }

        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/payments")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber)
        {
            var certificates = await _service.GetCertificatesAsync(new GetCertificatesRequest { SiNumber = sinumber });

            List<BobPayment> payments = new List<BobPayment>();

            foreach (var certificate in certificates?.Value?.Certificates)
            {
                var response = await _service.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = sinumber, ReferenceDate = certificate.From });
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);
                payments.AddRange(_mapper.Map<IEnumerable<BobPayment>>(response.Value?.Payments, opt => opt.Items["Id"] = certificate.Id));
            }

            return Ok(payments);
        }
    }
}