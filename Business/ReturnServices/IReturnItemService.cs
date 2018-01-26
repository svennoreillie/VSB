using VSBaseAngular.Models;
using System.Threading.Tasks;

namespace VSBaseAngular.Business
{
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
        
    }
}
