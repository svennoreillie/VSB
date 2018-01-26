using System;

namespace VSBaseAngular.Business
{
    public class ReturnCalculationService : IReturnCalculationService {

        //Aantal maanden maximum
        private const int maxReturnPeriod = 12;

        public int PaymentsBetweenDates(DateTime? until) {
            return PaymentsBetweenDates(until, DateTime.Today);
        }

        public int PaymentsBetweenDates(DateTime? until, DateTime start) {
            if (!until.HasValue) return maxReturnPeriod;
            int months = (until.Value.Year - DateTime.Today.Year ) * 12 + (until.Value.Month - DateTime.Today.Month );
            return Math.Max(0, Math.Min(months, maxReturnPeriod));
        }
    }
}