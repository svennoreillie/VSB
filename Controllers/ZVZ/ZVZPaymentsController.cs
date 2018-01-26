using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;
using ZvzService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ZVZPaymentsController : BaseController
    {
        private readonly IZvzService _service;
        private IMapper _mapper;

        public ZVZPaymentsController(IServiceFactory<IZvzService> zvzServiceFactory, IMapper mapper)
        {
            this._service = zvzServiceFactory.GetService();
            this._mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/zvzwarranties/{sinumber:long}/payments/{warrantyRequestDate:DateTime}")]
        public async Task<IActionResult> GetSpecific(long sinumber, DateTime warrantyRequestDate)
        {
            var response = await _service.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = sinumber, ReferenceDate = warrantyRequestDate });

            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var mappedModel = _mapper.Map<IEnumerable<ZvzPayment>>(response.Value?.Payments);
            return Ok(mappedModel);
        }

        [Route("~/api/v{version:apiVersion}/zvzwarranties/{sinumber:long}/payments")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber)
        {
            var warranties = await _service.GetWarrantiesAsync(new GetWarrantiesRequest { SiNumber = sinumber });
            if (warranties.BusinessMessages != null && warranties.BusinessMessages.Length > 0)
                return BadRequest(warranties.BusinessMessages);

            List<ZvzPayment> payments = new List<ZvzPayment>();

            foreach (var warranty in warranties?.Value?.Warranties)
            {
                var response = await _service.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = sinumber, ReferenceDate = warranty.RequestDate });
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);
                payments.AddRange(_mapper.Map<IEnumerable<ZvzPayment>>(response.Value?.Payments, opt => opt.Items["CertificateId"] = warranty.Certificate));
            }

            return Ok(payments);
        }

    }
}