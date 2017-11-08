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
                cfg.CreateMap<BobService.Certificate, BobCertificate>()
                    .ForMember(dest => dest.BeginDate, opt => opt.MapFrom(src => src.From))
                    .ForMember(dest => dest.CertificateId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Until))
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                    .ForMember(dest => dest.DecisionDate, opt => opt.MapFrom(src => src.DecisionDate))

                    .ForMember(dest => dest.TerminationStartDate, opt => opt.Ignore())
                    .ForMember(dest => dest.TerminationReason, opt => opt.Ignore())
                    .ForMember(dest => dest.TerminationEndDate, opt => opt.Ignore())

                    .AfterMap((src, dest) => {
                       if (src.EndPayment != null)  {
                           dest.TerminationStartDate = src.EndPayment?.From;
                           dest.TerminationReason = src.EndPayment?.Reason;
                           dest.TerminationEndDate = src.EndPayment?.Until;
                       }
                    });

            });

            config.AssertConfigurationIsValid();

            IMapper map = new Mapper(config);

            services.AddSingleton<IMapper>(map);
        }
    }
}