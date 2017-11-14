using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZvzService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ZVZContributionsController : BaseController
    {
        private readonly IZvzService _service;
        private readonly IMapper _mapper;

        public ZVZContributionsController(IServiceFactory<IZvzService> zvzServiceFactory, IMapper mapper)
        {
            this._service = zvzServiceFactory.GetService();
            this._mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetContributionsAsync(new GetContributionsRequest() { SiNumber = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            var mappedModel = _mapper.Map<IEnumerable<ZvzContribution>>(response.Value?.Contributions);
            return Ok(mappedModel);
        }

     
    }
}