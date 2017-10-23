using Microsoft.Extensions.DependencyInjection;
using VSBaseAngular.Business;
using VSBaseAngular.Models;

namespace VSBaseAngular {
    public static class DependencyInjection {
        public static void AddServices(IServiceCollection services) {


            services.AddTransient<IReader<Person>, PeopleReader>();

            
        }
    }
}