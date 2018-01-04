using VSBaseAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ThabService;

namespace VSBaseAngular.Business
{
    public class ReturnThabService : IReturnItemService
    {

        private IThabService _thabService;
        private IReturnCalculationService _helperService;
        private int maxPercentage = 10;

        public ReturnCalculationKind Kind
        {
            get { return ReturnCalculationKind.THAB; }
        }



        public ReturnThabService(IThabService thabService, IReturnCalculationService helper)
        {
            _thabService = thabService;
            _helperService = helper;
        }




        public async Task<decimal> GetMaxAmountReturnableAsync(long siNumber, string insz)
        {
            var response = await _thabService.GetCertificatesAsync(new GetCertificatesRequest() { SiNumber = siNumber, Insz = insz });
            return await CalcMaxAmount(siNumber, response);
        }

        public IEnumerable<ReturnCalculationPayment> GetPaymentLines(decimal totalAmount)
        {
            //TODO
            yield return new ReturnCalculationPayment()
            {
                Amount = totalAmount,
            };
        }




        private async Task<decimal> GetCurrentPaymentAmount(long siNumber, DateTime reference)
        {
            var request = new GetPaymentsRequest() { SiNumber = siNumber, ReferenceDate = reference };
            var response = await _thabService.GetPaymentsAsync(request);
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

        private async Task<decimal> CalcMaxAmount(long siNumber, ReturnMessageOfGetCertificatesResponsefjFlgFUb response)
        {
            if (response.Value != null && response.Value.Certificates != null)
            {

                var futureCertifcates = response.Value.Certificates.Where(c => !c.Until.HasValue || c.Until > DateTime.Today);

                decimal sum = 0;
                foreach (var fc in futureCertifcates)
                {
                    sum += _helperService.PaymentsBetweenDates(fc.Until) * (await GetCurrentPaymentAmount(siNumber, fc.InitialDate)) * maxPercentage;
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