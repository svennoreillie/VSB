using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business {
    public interface IUnReturnablePaymentLineService {
        IEnumerable<ReturnCalculationLine> GetUnreturnableLines(bool isFraude, IEnumerable<ReturnCalculationLine> lines);
        IEnumerable<ReturnCalculationLine> GetReturnableLines(bool isFraude, IEnumerable<ReturnCalculationLine> lines);
    }
}
