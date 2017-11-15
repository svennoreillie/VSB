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
        public IReader<Person> _peopleService { get; }

        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABPaymentsController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<Person> peopleService,
                                         IMapper mapper)
        {
            _peopleService = peopleService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/payments/{referenceDate:datetime}")]
        public IActionResult Get(long sinumber, DateTime referenceDate)
        {
            var request = new GW.VSB.THAB.Contracts.GetPayments.GetPaymentsRequest()
            {
                SiNumber = sinumber,
                ReferenceDate = referenceDate
            };
            var response = _service.GetPayments(request);
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var mappedModels = _mapper.Map<IEnumerable<THABPayment>>(response?.Value?.Payments);
            return Ok(mappedModels);
        }
    }
}
