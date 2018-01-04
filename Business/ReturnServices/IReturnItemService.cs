using VSBaseAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBaseAngular.Business {
    public interface IReturnItemService {
        /// <summary>
        /// The kind of service with which this instance interacts
        /// </summary>
        ReturnCalculationKind Kind { get; }

        /// <summary>
        /// Calculates the maximum deductable amount for a given person by combining the known services together
        /// </summary>
        /// <param name="siNumber">The siNumber of the person for which to calculate the maximum deductable amount</param>
        /// <returns>The maximum amount that can be returned</returns>
        /// <exception cref="ArgumentException">Throws exception when the given siNumber is invalid</exception>
        Task<decimal> GetMaxAmountReturnableAsync(long siNumber, string insz);
        

        /// <summary>
        /// Gets the paymentlines which will be used for returning the given amount
        /// </summary>
        /// <param name="totalAmount">The amount that needs to be returned</param>
        /// <returns>A collection of payments on which the amoun can be returned</returns>
        IEnumerable<ReturnCalculationPayment> GetPaymentLines(decimal totalAmount);
    }
}
