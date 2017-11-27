using System;
using System.Collections.Generic;
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
    public class ZVZLettersController : BaseController
    {
        private readonly IZvzService _service;
        private readonly IMapper _mapper;

        public ZVZLettersController(IServiceFactory<IZvzService> bobServiceFactory, IMapper mapper)
        {
            _service = bobServiceFactory.GetService();
            _mapper = mapper;
        }



        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery]short federation,
                                    [FromQuery]int membernr,
                                    [FromQuery]int sex,
                                    [FromQuery]DateTime birthdate,
                                    [FromQuery]long sinumber) {
            var request = new GetDecisionLettersRequest
            {
                BirthDate = birthdate,
                SiNumber = sinumber,
                Fed = federation,
                FedNumber = membernr,
                Sex =  sex
            };
            var response = await _service.GetDecisionLetterAsync(request);

            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var mappedModel = _mapper.Map<IEnumerable<Letter>>(response.Value?.DecisionLetters);

            return Ok(mappedModel);
        }
    }
}