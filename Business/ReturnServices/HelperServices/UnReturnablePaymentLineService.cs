using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business {
    public class UnReturnablePaymentLineService : IUnReturnablePaymentLineService {

       

        public IEnumerable<ReturnCalculationLine> GetUnreturnableLines(bool isFraude, IEnumerable<ReturnCalculationLine> lines) {
            if (lines == null) throw new ArgumentNullException("lines");
            if (lines.Count() == 0) yield break;

            DateTime referenceDate = GetReferenceDate(isFraude);

            foreach (var line in lines) {
                ReturnCalculationLine returnLine = new ReturnCalculationLine(line.Kind);
                returnLine.PaymentLines.AddRange(line.PaymentLines.Where(pl => {
                    if (pl.SendDate.HasValue) return pl.SendDate < referenceDate;
                    else if (pl.StartDate.HasValue) return pl.StartDate < referenceDate;
                    else if (pl.EndDate.HasValue) return pl.EndDate < referenceDate;
                    return false;
                }));
                yield return returnLine;
            }
        }

        public IEnumerable<ReturnCalculationLine> GetReturnableLines(bool isFraude, IEnumerable<ReturnCalculationLine> lines) {
            if (lines == null) throw new ArgumentNullException("lines");
            if (lines.Count() == 0) yield break;

            DateTime referenceDate = GetReferenceDate(isFraude);

            foreach (var line in lines) {
                ReturnCalculationLine returnLine = new ReturnCalculationLine(line.Kind);
                returnLine.PaymentLines.AddRange(line.PaymentLines.Where(pl => {
                    if (pl.SendDate.HasValue) return pl.SendDate >= referenceDate;
                    else if (pl.StartDate.HasValue) return pl.StartDate >= referenceDate;
                    else if (pl.EndDate.HasValue) return pl.EndDate >= referenceDate;
                    return true;
                }));
                yield return returnLine;
            }
        }

        private DateTime GetReferenceDate(bool isFraude) {
            if (isFraude) return DateTime.Today.AddYears(-5);
            return DateTime.Today.AddYears(-3);
        }

    }


}