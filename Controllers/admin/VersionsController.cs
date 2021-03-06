using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VSBaseAngular;
using VSBaseAngular.Helpers.Options;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class VersionsController : Controller
    {
        private readonly AppConfig _config;

        public VersionsController(IOptions<AppConfig> config)
        {
            _config = config.Value;
        }

        public IActionResult Get()
        {
            Version version = new Version(_config.Version);
            return Ok(new { Version= version.ToString()});
        }
    }
}