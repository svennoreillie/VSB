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
    public class THABPayableAmountsController : BaseController
    {
        public IReader<Person> _peopleService { get; }

        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABPayableAmountsController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<Person> peopleService,
                                         IMapper mapper)
        {
            _peopleService = peopleService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/payableamounts/{referenceDate:datetime}")]
        public IActionResult Get(long sinumber, string certificateid)
        {
            var request = new GW.VSB.THAB.Contracts.GetCertificates.GetCertificatesPayableAmountRequest()
            {
                SiNumber = sinumber,
                Id = certificateid
            };
            var response = _service.GetCertificatesPayableAmounts(request);
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var mappedModels = _mapper.Map<IEnumerable<ThabPayableAmount>>(response?.Value?.PayableAmounts);
            return Ok(mappedModels);
        }
    }
}
