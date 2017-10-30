using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ZVZWarrentiesController : BaseController
    {
        private readonly IReader<Person> _peopleReader;

        public ZVZWarrentiesController(IReader<Person> peopleReader)
        {
            _peopleReader = peopleReader;
        }



    }
}