using System.Threading.Tasks;
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

        public BOBPaymentsController(IServiceFactory<IBobService> bobServiceFactory)
        {
            _service = bobServiceFactory.GetService();
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/payments")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = sinumber});
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            return Ok(response.Value?.Payments);
        }
    }
}