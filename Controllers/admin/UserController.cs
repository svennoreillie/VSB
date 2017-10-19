using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/admin/[controller]")]
public class UserController : Controller
{
    [Authorize]
    public IActionResult Get() {
        //TODO
        return Ok(User.Identity.Name);  
    }
}