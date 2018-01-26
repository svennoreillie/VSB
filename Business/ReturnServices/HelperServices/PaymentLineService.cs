using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business {
    public class PaymentLineService : IPaymentLineService {

        private  IUnReturnablePaymentLineService _unreturnablePaymentLineService;

        public PaymentLineService(IUnReturnablePaymentLineService unreturnablePaymentLineService) {
            _unreturnablePaymentLineService = unreturnablePaymentLineService;
        }

        public decimal GetUnreturnableAmount(bool isFraude, IEnumerable<ReturnCalculationLine> lines) {
            if (lines == null) throw new ArgumentNullException("lines");
            IEnumerable<ReturnCalculationLine> filteredLines = _unreturnablePaymentLineService.GetUnreturnableLines(isFraude, lines);
            return CalculateSum(filteredLines.Where(pl => pl.Kind != ReturnCalculationKind.THAB_FOD));
        }

        public decimal GetReturnableAmount(bool isFraude, IEnumerable<ReturnCalculationLine> lines) {
            if (lines == null) throw new ArgumentNullException("lines");
            IEnumerable<ReturnCalculationLine> filteredLines = _unreturnablePaymentLineService.GetReturnableLines(isFraude, lines);
            return CalculateSum(filteredLines);
        }

        public decimal GetReturnableAmountByFOD(bool isFraude, IEnumerable<ReturnCalculationLine> lines) {
            if (lines == null) throw new ArgumentNullException("lines");
            IEnumerable<ReturnCalculationLine> filteredLines =  _unreturnablePaymentLineService.GetUnreturnableLines(isFraude, lines);
            return CalculateSum(filteredLines.Where(pl => pl.Kind == ReturnCalculationKind.THAB_FOD));
        }

        private decimal CalculateSum(IEnumerable<ReturnCalculationLine> lines) {
            if (lines == null) return 0;
            return lines.Sum(ul => ul.PaymentLines.Sum(pl => pl.Amount));
        }
               

    }


}