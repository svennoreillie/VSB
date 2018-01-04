using VSBaseAngular.Models;
using System;
namespace VSBaseAngular.Business.ReturnServices {
    public interface IReturnValidationService {
        Result<bool> Validate(VSBaseAngular.Models.ReturnCalculationRequest request);
    }
}
