using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VSBaseAngular;

namespace VSBaseAngular.Controllers
{
    [Route("api/admin/[controller]")]
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
            return Ok(version.ToString());
        }
    }
}