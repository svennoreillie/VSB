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
    public class THABRemarksController : BaseController
    {
        public IReader<Person> _peopleService { get; }

        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABRemarksController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<Person> peopleService,
                                         IMapper mapper)
        {
            _peopleService = peopleService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/remarks/{referenceDate:datetime}")]
        public async Task<IActionResult> Get(long sinumber, DateTime referenceDate)
        {
            var remarkRequest = new GetRemarkRequest()
            {
                SiNumber = sinumber,
                ReferenceDate = referenceDate
            };
            var remarkResponse = await _service.GetRemarkAsync(remarkRequest);
            if (remarkResponse.BusinessMessages != null && remarkResponse.BusinessMessages.Length > 0)
                return BadRequest(remarkResponse.BusinessMessages);

            return Ok(remarkResponse?.Value?.Remark);
        }

        
    }
}
