using Microsoft.AspNetCore.Mvc;

[Route("api/admin/[controller]")]
public class VersionController : Controller
{
    public IActionResult Get() {
        //TODO
        string version = "0.2.0";
        return Ok(version);  
    }
}