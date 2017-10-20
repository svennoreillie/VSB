using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VSBaseAngular;

namespace VSBaseAngular.Controllers
{
    [Route("api/admin/[controller]")]
    public class VersionController : Controller
    {
        private readonly AppConfig _config;

        public VersionController(IOptions<AppConfig> config)
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