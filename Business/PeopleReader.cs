using System.Collections.Generic;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Search;
using VSBaseAngular.Models.Keys;
using System.Threading.Tasks;

namespace VSBaseAngular.Business
{
    public class PeopleReader : IReader<Person>
    {
        public async Task<Person> GetAsync(KeySet<Person> key)
        {
            var pKey = key as PersonKey;
            System.Console.WriteLine("Get key" + pKey.SiNumber);

            return await Task.FromResult<Person>(new Person());
        }

        public async Task<IEnumerable<Person>> SearchAsync(SearchBaseModel<Person> model)
        {
            System.Console.WriteLine("Search person");

            return await Task.FromResult(new List<Person>() { new Person { FirstName = "Sven", Name = "Noreillie" } });
        }
    }
}