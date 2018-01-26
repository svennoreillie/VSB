using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Business.ReturnServices;

namespace VSBaseAngular {
    public static class DependencyInjection {
        public static void AddServices(IServiceCollection services) {

            //Httpclient should be a singleton => memory leaks if ever deployed on linux
            services.AddSingleton<HttpClient>(new HttpClient());

            services.AddTransient<IReader<Person>, PeopleReader>();
            services.AddTransient<IReader<User>, UserReader>();
            services.AddTransient<IReader<ThabCertificate>, ThabCertificateReader>();

            services.AddTransient<IReturnItemService, ReturnBobService>();
            services.AddTransient<IReturnItemService, ReturnZvzService>();
            services.AddTransient<IReturnItemService, ReturnThabService>();

            services.AddTransient<IPaymentLineService, PaymentLineService>();
            services.AddTransient<IReturnPropositionService, ReturnPropositionService>();
            services.AddTransient<IReturnCalculationDataService, ReturnCalculationDataService>();
            services.AddTransient<IReturnCalculationService, ReturnCalculationService>();
            services.AddTransient<IReturnValidationService, ReturnValidationService>();
            services.AddTransient<IUnReturnablePaymentLineService, UnReturnablePaymentLineService>();
            
            services.AddTransient(typeof(IServiceFactory<>), typeof(ServiceFactory<>));
            
        }
    }
}