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
    public class BOBCertificatesController : BaseController
    {
        private readonly IBobService _service;

        public BOBCertificatesController(IServiceFactory<IBobService> bobServiceFactory)
        {
            _service = bobServiceFactory.GetService();
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/certificates")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetCertificatesAsync(new GetCertificatesRequest() { SiNumber = sinumber});
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            return Ok(response.Value?.Certificates);
        }
    }
}