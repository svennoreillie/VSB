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
                        dest.TerminationStartDate = src.EndPayment?.From;
                        dest.TerminationReason = src.EndPayment?.Reason;
                        dest.TerminationEndDate = src.EndPayment?.Until;
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

                cfg.CreateMap<ZvzService.DecisionLetter, ZvzLetter>()
                    .ForMember(dest => dest.LetterDate, opt => opt.MapFrom(src => src.SendedDate))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));


                cfg.CreateMap<ZvzService.Contract, ZvzContract>()
                    .ForMember(dest => dest.CloseDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(dest => dest.CloseMotive, opt => opt.MapFrom(src => src.EndMotivation))
                    .ForMember(dest => dest.CloseReason, opt => opt.MapFrom(src => src.EndReason))
                    .ForMember(dest => dest.ContractDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(dest => dest.ContractMotive, opt => opt.MapFrom(src => src.StartMotivation))
                    .ForMember(dest => dest.ContractReason, opt => opt.MapFrom(src => src.StartReason));

                cfg.CreateMap<ZvzService.Warranty, ZvzWarranty>()
                    .ForMember(dest => dest.CareForm, opt => opt.MapFrom(src => src.Care))
                    .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.Certificate))
                    .ForMember(dest => dest.DecisionDate, opt => opt.MapFrom(src => src.DecisionDate))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.DateUntil))
                    .ForMember(dest => dest.RefusalReason, opt => opt.MapFrom(src => src.RefusalReason))
                    .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.RequestDate))
                    .ForMember(dest => dest.Scale, opt => opt.MapFrom(src => src.Scale))
                    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.DateFrom))
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

                cfg.CreateMap<ZvzService.Payment, ZvzPayment>()
                    .ForMember(dest => dest.AccountNb, opt => opt.MapFrom(src => src.Account != null ? src.Account.Iban : null))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.BeginDate, opt => opt.MapFrom(src => src.PeriodStart))
                    .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.PeriodEnd))
                    .ForMember(dest => dest.SendDate, opt => opt.MapFrom(src => src.SendDate))
                    .ForMember(dest => dest.UnCode, opt => opt.MapFrom(src => src.UnCode));


                cfg.CreateMap<ZvzService.Contribution, ZvzContribution>()
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.Circuit, opt => opt.MapFrom(src => src.Circuit))
                    .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                    .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                    .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year));

                //thab
                cfg.CreateMap<GW.VSB.THAB.Contracts.Data.Certificate, ThabCertificate>()
                    .ForMember(dest => dest.BeginDate, opt => opt.MapFrom(src => src.From))
                    .ForMember(dest => dest.CertificateId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.DecisionDate, opt => opt.MapFrom(src => src.DecisionDate))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Until))
                    .ForMember(dest => dest.IsMigrated, opt => opt.MapFrom(src => src.MigrateDate.HasValue))
                    .ForMember(dest => dest.MigrateDate, opt => opt.MapFrom(src => src.MigrateDate))
                    .ForMember(dest => dest.ReferenceDate, opt => opt.MapFrom(src => src.InitialDate))
                    .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                    .ForMember(dest => dest.Remark, opt => opt.Ignore())
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                    .ForMember(dest => dest.TerminationReason, opt => opt.MapFrom(src => src.ReasonClosure));

                cfg.CreateMap<GW.VSB.THAB.Contracts.Data.Payment, ThabPayment>()
                    .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                    .ForMember(dest => dest.SendDate, opt => opt.MapFrom(src => src.SendDate))
                    .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.Account != null ? src.Account.Iban : null))
                    .ForMember(dest => dest.PeriodStart, opt => opt.MapFrom(src => src.PeriodStart))
                    .ForMember(dest => dest.PeriodEnd, opt => opt.MapFrom(src => src.MigrateDate.PeriodEnd))
                    .ForMember(dest => dest.UnCode, opt => opt.MapFrom(src => src.UnCode));

                cfg.CreateMap<GW.VSB.THAB.Contracts.Data.CertificatePayableAmount, ThabPayableAmount>()
                    .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start))
                    .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            });

            config.AssertConfigurationIsValid();

            IMapper map = new Mapper(config);

            services.AddSingleton<IMapper>(map);
        }
    }
}