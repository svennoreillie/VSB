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
    public class PeopleController : BaseController
    {
        private readonly IReader<Person> _peopleReader;

        public PeopleController(IReader<Person> peopleReader)
        {
            _peopleReader = peopleReader;
        }


        [HttpGet]
        //[Route("{sinumber:string}")]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var key = new PersonKey("sinumber");
            Person  p = await _peopleReader.GetAsync(key);
            return Ok(p);
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery]string firstname,
                                    [FromQuery]string name,
                                    [FromQuery]string insz,
                                    [FromQuery]string membernr)
        {
            PersonSearch model = new PersonSearch();
            
            var people = await _peopleReader.SearchAsync(model);

            return Ok(people);
        }

    }
}