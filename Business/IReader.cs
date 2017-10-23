using System.Collections.Generic;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Search;
using VSBaseAngular.Models.Keys;
using System.Threading.Tasks;

namespace VSBaseAngular.Business
{
    public interface IReader<T> where T : ModelBase
    {

        Task<T> GetAsync(KeySet<T> key);
        Task<IEnumerable<T>> SearchAsync(SearchBaseModel<T> model);

    }
}