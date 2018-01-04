using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public class ReturnCalculationResponse {

        public ReturnCalculationResponse() {
            this.ReturnLines = new List<ReturnCalculationLine>();
        }

        public List<ReturnCalculationLine> ReturnLines { get; set; }
        public decimal AmountRefundableByOGM { get; set; }
        public decimal AmountNonRefundable { get; set; }


        //Calculated properties
        public ReturnCalculationType Type {
            get {
                if (this.TotalAmount == 0) return ReturnCalculationType.NothingToReturn;
                if (this.AmountRefundableByDeduction == this.TotalAmount) return ReturnCalculationType.FullDeduction;
                if (this.AmountRefundableByOGM == this.TotalAmount) return ReturnCalculationType.NonDeductable;
                return ReturnCalculationType.PartialDeduction;
            }
        }

        public decimal TotalAmount {
            get {
                return this.AmountRefundableByDeduction + this.AmountRefundableByOGM;
            }
        }

        public decimal AmountRefundableByDeduction {
            get {
                if (this.ReturnLines == null) return 0;
                if (!this.ReturnLines.Any()) return 0;
                return ReturnLines.Where(l => l.PaymentLines != null)
                                  .Sum(l => l.PaymentLines.Sum(pl => pl.Amount));
            }
        }
        
    }
}