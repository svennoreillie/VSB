using Microsoft.AspNetCore.Mvc;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [ApiVersion(ControllerVersion.v2)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class PeopleController : BaseController
    {
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok(id);
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get([FromQuery]string firstname,
                                    [FromQuery]string name,
                                    [FromQuery]string insz,
                                    [FromQuery]string membernr)
        {
            return Ok($"{firstname}-{name}-{insz}-{membernr}");
        }

    }
}