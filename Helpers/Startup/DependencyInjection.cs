using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using VSBaseAngular.Business;
using VSBaseAngular.Models;

namespace VSBaseAngular {
    public static class DependencyInjection {
        public static void AddServices(IServiceCollection services) {

            //Httpclient should be a singleton => memory leaks if ever deployed on linux
            services.AddSingleton<HttpClient>(new HttpClient());

            services.AddTransient<IReader<Person>, PeopleReader>();
            services.AddTransient<IReader<User>, UserReader>();
            services.AddTransient<IReader<ThabCertificate>, ThabCertificateReader>();
            
            services.AddTransient(typeof(IServiceFactory<>), typeof(ServiceFactory<>));
            
        }
    }
}