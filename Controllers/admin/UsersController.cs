using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VSBaseAngular.Business;
using VSBaseAngular.Models;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class UsersController : Controller
    {
        private readonly IReader<User> reader;

        public UsersController(IReader<User> userReader)
        {
            this.reader = userReader;
        }

        public async Task<IActionResult> Get()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username)) return null;
            var user = await this.reader.GetAsync(new Models.Keys.UserKey(username.Split('\\')[0], username.Split('\\')[1]));
            return Ok(new { User = user?.DisplayName });
        }
    }
}
