using ZvzService;
using VSBaseAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSBaseAngular.Business
{

    public class ReturnZvzService : IReturnItemService {

        private  IZvzService _zvzService;
        private  IReturnCalculationService _helperService;

        public ReturnCalculationKind Kind {
            get { return ReturnCalculationKind.ZVZ; }
        }


        public ReturnZvzService(IServiceFactory<IZvzService> zvzServiceFactory, IReturnCalculationService helper) {
            _zvzService = zvzServiceFactory.GetService();
            _helperService = helper;
        }



        public async Task<decimal> GetMaxAmountReturnableAsync(long siNumber, string insz) {
            var response = await _zvzService.GetWarrantiesAsync(new GetWarrantiesRequest() { SiNumber = siNumber });
            return await CalcMaxAmount(siNumber, response);
        }


        private async Task<decimal> GetCurrentPaymentAmount(long siNumber, DateTime reference) {
            var response = await _zvzService.GetPaymentsAsync(new GetPaymentsRequest() { SiNumber = siNumber, ReferenceDate = reference });
            if (response.Value != null && response.Value.Payments != null && response.Value.Payments.Count() > 0) {
                return response.Value.Payments.OrderByDescending(p => p.PeriodEnd).FirstOrDefault().Amount;
            } else if (response.BusinessMessages != null && response.BusinessMessages.Any()) {
                var errors = response.BusinessMessages.Where(bm => bm.Type == MessageType.Error);
                if (errors.Any()) {
                    throw new Exception(string.Join("; ", errors.Select(e => e.MessageString)));
                }
            }
            return 0;
        }

        private async Task<decimal> CalcMaxAmount(long siNumber, ReturnMessageOfGetWarrantiesResponsecEfhslJU response) {
            if (response.Value != null && response.Value.Warranties != null && response.Value.Warranties.Count() > 0) {

                var lastWarranty = response.Value.Warranties.OrderByDescending(w => w.DateFrom).FirstOrDefault();
                if (lastWarranty == null) return 0;

                var enddate = lastWarranty.DateUntil;
                return _helperService.PaymentsBetweenDates(enddate) * await GetCurrentPaymentAmount(siNumber, lastWarranty.RequestDate);

            } else if (response.BusinessMessages != null && response.BusinessMessages.Any()) {
                var errors = response.BusinessMessages.Where(bm => bm.Type == MessageType.Error);
                if (errors.Any()) {
                    throw new Exception(string.Join("; ", errors.Select(e => e.MessageString)));
                }
            }
            return 0;
        }
    }
}