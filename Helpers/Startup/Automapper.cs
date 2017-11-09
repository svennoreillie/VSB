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

                    .AfterMap((src, dest) =>
                    {
                        if (src.EndPayment != null)
                        {
                            dest.TerminationStartDate = src.EndPayment?.From;
                            dest.TerminationReason = src.EndPayment?.Reason;
                            dest.TerminationEndDate = src.EndPayment?.Until;
                        }
                    });

                cfg.CreateMap<BobService.Payment, BobPayment>()
                    .ForMember(dest => dest.AccountNb, opt => opt.Ignore())
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.BeginDate, opt => opt.MapFrom(src => src.PeriodStart))
                    .ForMember(dest => dest.CertificateId, opt => opt.ResolveUsing((src, dest, value, context) => context.Options.Items["Id"]))
                    .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.PeriodEnd))
                    .ForMember(dest => dest.SendDate, opt => opt.MapFrom(src => src.SendDate))
                    .ForMember(dest => dest.UnCode, opt => opt.MapFrom(src => src.UnCode))
                    .AfterMap((src, dest) =>
                    {
                        dest.AccountNb = src.Account?.Iban;
                    });

                cfg.CreateMap<BobService.Letter, BobLetter>()
                    .ForMember(dest => dest.LetterDate, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));


            });

            config.AssertConfigurationIsValid();

            IMapper map = new Mapper(config);

            services.AddSingleton<IMapper>(map);
        }
    }
}