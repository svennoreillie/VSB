using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABCertificatesController : BaseController
    {
        private readonly IThabService _service;

        public THABCertificatesController(IServiceFactory<IThabService> thabServiceFactory)
        {
            _service = thabServiceFactory.GetService();
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = _service.GetCertificates(new GW.VSB.THAB.Contracts.GetCertificates.GetCertificatesRequest() { SiNumber = sinumber, Insz = "75111904963" });
            return Ok(response);
        }
    }
}