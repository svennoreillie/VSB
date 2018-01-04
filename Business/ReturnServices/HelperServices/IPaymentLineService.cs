using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business {
    public interface IPaymentLineService {
        decimal GetUnreturnableAmount(bool isFraude, IEnumerable<ReturnCalculationLine> lines);
        decimal GetReturnableAmount(bool isFraude, IEnumerable<ReturnCalculationLine> lines);
    }
}
