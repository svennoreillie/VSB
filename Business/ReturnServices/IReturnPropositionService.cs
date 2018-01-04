using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business {
    public interface IReturnPropositionService {

        Task<(List<ReturnCalculationLine> lines, decimal amount)> DistributeAmount(decimal amount, long siNumber, string insz);

        /// <summary>
        /// Calculates the maximum deductable amount for a given person by combining the known services together
        /// </summary>
        /// <param name="siNumber">The siNumber of the person for which to calculate the maximum deductable amount</param>
        /// <returns>The maximum amount</returns>
        /// <exception cref="ArgumentException">Throws exception when the given siNumber is invalid</exception>
        Task<decimal> CalculateMaximumDeductableAmount(long siNumber, string insz);

        /// <summary>
        /// Calculates a default proposition for returning a given amount for a given person
        /// </summary>
        /// <param name="request">Contains the lines which need to be returned and a person for which these lines exist</param>
        /// <returns>A proposition response</returns>
        /// <exception cref="ArgumentNullException">Throws exception when request is null</exception>
        /// <exception cref="ArgumentException">Throws exception when request has invalid properties</exception>
        Task<Result<ReturnCalculationResponse>> CalculateDefaultProposition(ReturnCalculationRequest request);
    }
}
