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
    public class BOBPeopleController : BaseController
    {
        private readonly IBobService _service;
        private readonly IMapper _mapper;

        public BOBPeopleController(IServiceFactory<IBobService> bobServiceFactory, IMapper mapper)
        {
            _service = bobServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetPersonAsync(new GetPersonRequest() { SiNumber = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            return Ok(response.Value?.person);
        }

        [HttpGet]
        [Route("{sinumber:long}/account")]
        public async Task<IActionResult> GetBobAccount(long sinumber)
        {
            var response = await _service.GetPersonAsync(new GetPersonRequest() { SiNumber = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            return Ok(response.Value?.person?.CurrentBOBAccountNr);
        }

        [HttpGet]
        [Route("{sinumber:long}/address")]
        public async Task<IActionResult> GetBobAddress(long sinumber)
        {
            var response = await _service.GetPersonAsync(new GetPersonRequest() { SiNumber = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            return Ok(response.Value?.person?.mainAddress);
        }

        [HttpGet]
        [Route("{sinumber:long}/contact")]
        public async Task<IActionResult> GetBobContact(long sinumber)
        {
            var response = await _service.GetPersonAsync(new GetPersonRequest() { SiNumber = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);

            var mappedModel = _mapper.Map<BobContact>(response.Value?.person?.contactData);
            return Ok(mappedModel);
        }
    }
}