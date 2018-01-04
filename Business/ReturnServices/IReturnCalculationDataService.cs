using VsbService;
using VSBaseAngular.Models;
using System;
using System.Threading.Tasks;

namespace VSBaseAngular.Business.ReturnServices {
    public interface IReturnCalculationDataService {
        Task<Result<bool>> StoreReturnCalculationAsync(ReturnCalculationRequest request);
        Task<Result<GetReturnCalculationResponse>> GetReturnCalculationAsync(string SiNumber);
    }
}
