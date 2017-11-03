using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VSBaseAngular.Business;
using VSBaseAngular.Models;

namespace VSBaseAngular
{
    public static class Automapper
    {
        public static void AddMapper(IServiceCollection services)
        {

            var config = new MapperConfiguration(cfg =>
            {
                
            });

            config.AssertConfigurationIsValid();

            IMapper map = new Mapper(config);

            services.AddSingleton<IMapper>(map);
        }
    }
}