using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSBaseAngular.Controllers
{
    [Route("api/admin/[controller]")]
    public class UserController : Controller
    {
        [Authorize]
        public IActionResult Get()
        {
            //TODO
            return Ok(User.Identity.Name);
        }
    }
}