using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSBaseAngular.Business {
    public interface IReturnCalculationService {
        int PaymentsBetweenDates(DateTime? until);
        int PaymentsBetweenDates(DateTime? until, DateTime start);
    }
}
