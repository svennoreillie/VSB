using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VSBaseAngular;
using VSBaseAngular.Helpers.Options;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class EnvironmentController : Controller
    {
        private readonly AppConfig _config;

        public EnvironmentController(IOptions<AppConfig> config)
        {
            _config = config.Value;
        }

        public IActionResult Get()
        {
            int env = _config.Environment;
            return Ok(new { Environment = env});
        }
    }
}