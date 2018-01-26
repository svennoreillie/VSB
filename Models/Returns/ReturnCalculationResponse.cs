using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public class ReturnCalculationResponse {

        public ReturnCalculationResponse() {
            this.ReturnLines = new List<ReturnCalculationResultLine>();
        }

        public List<ReturnCalculationResultLine> ReturnLines { get; set; }
        public decimal AmountRefundableByOGM { get; set; }
        public decimal AmountRefundableByFOD { get; set; }
        public decimal AmountNonRefundable { get; set; }


        //Calculated properties
        public ReturnCalculationType Type {
            get {
                if (this.TotalAmount == 0) return ReturnCalculationType.NothingToReturn;
                if (this.AmountRefundableByDeduction == this.TotalAmount) return ReturnCalculationType.FullDeduction;
                if (this.AmountRefundableByOGM == this.TotalAmount) return ReturnCalculationType.NonDeductable;
                throw new Exception("Both OGM and Decuction is no longer possible");
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
                return ReturnLines.Sum(l => l.Amount);
            }
        }
        
    }
}