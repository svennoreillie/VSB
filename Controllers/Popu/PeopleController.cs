using System;
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
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var key = new PersonKey(sinumber);
            Person p = await _peopleReader.GetAsync(key);
            return Ok(p);
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery]string firstname,
                                    [FromQuery]string name,
                                    [FromQuery]string insz,
                                    [FromQuery]int federation,
                                    [FromQuery]string membernr,
                                    [FromQuery]long sinumber,
                                    [FromQuery]int skip,
                                    [FromQuery]int limit = int.MaxValue)
        {
            PersonSearch model = new PersonSearch();
            model.Federation = federation;
            model.FirstName = firstname;
            model.Insz = insz;
            model.MemberNr = membernr;
            model.Name = name;
            model.SiNumber = sinumber;

            model.Limit = limit;
            model.Skip = skip;

            var people = await _peopleReader.SearchAsync(model);

            return Ok(people);
        }

    }
}