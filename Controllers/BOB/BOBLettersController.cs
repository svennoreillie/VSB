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
    public class BOBLettersController : BaseController
    {
        private readonly IBobService _service;

        public BOBLettersController(IServiceFactory<IBobService> bobServiceFactory)
        {
            _service = bobServiceFactory.GetService();
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber}/certificates/{certificateId}/leters")]
        [Route("~/api/v{version:apiVersion}/bobcertificates/{certificateId}/leters")]
        [Route("{certificateId}")]
        public async Task<IActionResult> Get(string certificateId)
        {
            var response = await _service.GetLettersAsync(new GetLettersRequest() { CertificateId = certificateId});
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            return Ok(response.Value?.Letters);
        }
    }
}