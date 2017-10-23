using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class UsersController : Controller
    {
        [Authorize]
        public IActionResult Get()
        {
            //TODO
            return Ok(User.Identity.Name);
        }
    }
}