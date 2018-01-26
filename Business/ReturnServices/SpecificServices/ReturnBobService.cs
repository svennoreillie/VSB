using VSBaseAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobService;

namespace VSBaseAngular.Business
{
    public class ReturnBobService : IReturnItemService
    {
        private IBobService _bobService;
        private IReturnCalculationService _helperService;

        public ReturnCalculationKind Kind
        {
            get { return ReturnCalculationKind.BOB; }
        }



        public ReturnBobService(IServiceFactory<IBobService> bobServiceFactory, IReturnCalculationService helper)
        {
            _bobService = bobServiceFactory.GetService();
            _helperService = helper;
        }



        public async Task<decimal> GetMaxAmountReturnableAsync(long siNumber, string insz)
        {
            ReturnMessageOfGetCertificatesResponse7nXz08R8 response = await _bobService.GetCertificatesAsync(new GetCertificatesRequest() { SiNumber = siNumber });
            return await CalcMaxAmount(siNumber, response);
        }


        private async Task<decimal> GetCurrentPaymentAmount(long siNumber, DateTime reference)
        {
            var response = await _bobService.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = siNumber, ReferenceDate = reference });
            if (response.Value != null && response.Value.Payments != null && response.Value.Payments.Count() > 0)
            {
                return response.Value.Payments.OrderByDescending(p => p.PeriodEnd).FirstOrDefault().Amount;
            }
            else if (response.BusinessMessages != null && response.BusinessMessages.Any())
            {
                var errors = response.BusinessMessages.Where(bm => bm.Type == MessageType.Error);
                if (errors.Any())
                {
                    throw new Exception(string.Join("; ", errors.Select(e => e.MessageString)));
                }
            }
            return 0;
        }

        private async Task<decimal> CalcMaxAmount(long siNumber, ReturnMessageOfGetCertificatesResponse7nXz08R8 response)
        {
            if (response.Value != null && response.Value.Certificates != null)
            {

                var futureCertifcates = response.Value.Certificates.Where(c => !c.Until.HasValue || c.Until > DateTime.Today);

                decimal sum = 0;
                foreach (var fc in futureCertifcates)
                {
                    sum += _helperService.PaymentsBetweenDates(fc.Until) * await GetCurrentPaymentAmount(siNumber, fc.From);
                }

            }
            else if (response.BusinessMessages != null && response.BusinessMessages.Any())
            {
                var errors = response.BusinessMessages.Where(bm => bm.Type == MessageType.Error);
                if (errors.Any())
                {
                    throw new Exception(string.Join("; ", errors.Select(e => e.MessageString)));
                }
            }
            return 0;
        }
    }
}