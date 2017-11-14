using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZvzService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ZVZContractsController : BaseController
    {
        private readonly IZvzService _service;
        private readonly IMapper _mapper;

        public ZVZContractsController(IServiceFactory<IZvzService> zvzServiceFactory, IMapper mapper)
        {
            this._service = zvzServiceFactory.GetService();
            this._mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetContractAsync(new GetLastContractRequest() { SiNumber = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            var mappedModel = _mapper.Map<ZvzContract>(response.Value?.LastContract);
            return Ok(mappedModel);
        }

     
    }
}