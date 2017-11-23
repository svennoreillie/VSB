using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class UsersController : Controller
    {
        public IActionResult Get()
        {
            return Ok(new { User = User.Identity.Name ?? "TODO" });
        }
    }
}
