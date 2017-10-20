using Microsoft.AspNetCore.Mvc;

namespace VSBAngular.Controllers
{
    [Route("api/[Controller]")]
    public class PeopleController : Controller
    {
        public IActionResult Get(int id)
        {
            return Ok();
        }

        public IActionResult Search([FromQuery]string firstname,
                                    [FromQuery]string name,
                                    [FromQuery]string insz,
                                    [FromQuery]string membernr)
        {
            return Ok($"{firstname}-{name}-{insz}-{membernr}");
        }
    }
}