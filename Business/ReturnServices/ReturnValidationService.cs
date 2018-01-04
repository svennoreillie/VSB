using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business.ReturnServices {
    public class ReturnValidationService : IReturnValidationService {
        private  IUnReturnablePaymentLineService _unreturnablePaymentLineService;

        public ReturnValidationService(IUnReturnablePaymentLineService unreturnablePaymentLineService) {
            _unreturnablePaymentLineService = unreturnablePaymentLineService;
        }

        public Result<bool> Validate(ReturnCalculationRequest request) {
            Result<bool> result = new Result<bool>();
            result.Code = ResultCode.Error;
            result.Value = false;

            if (request == null) return result.AddMessage("Request is null");
            if (request.SiNumber <= 0) return result.AddMessage("SiNumber must be 1 or higher");
            if (request.SiNumber > 99999999999) return result.AddMessage("SiNumber is too large");
            if (string.IsNullOrWhiteSpace(request.Reason)) return result.AddMessage("Reason not provided");
            if (string.IsNullOrWhiteSpace(request.Insz)) return result.AddMessage("INSZ not provided");

            if (request.ReturnByDeduction > 0
                || request.ReturnByOGM > 0) {
                //Proposition was altered

                decimal sum = request.ReturnByOGM + request.ReturnByDeduction;
                var lines = _unreturnablePaymentLineService.GetReturnableLines(request.IsFraude, request.ReturnLines);

                if (lines.Sum(rl => rl.PaymentLines.Sum(pl => pl.Amount)) != sum) return result.AddMessage("Total returnable amount does not match custom proposition");
            }

            result.Code = ResultCode.Ok;
            result.Value = true;

            return result;
        }

        
    }

    public static class RangeExtentions {
        public static bool IsInRangeInclusive(this int value, int lower, int higher) {
            return (value >= lower && value <= higher);
        }

        public static bool IsInRangeExclusive(this int value, int lower, int higher) {
            return ( value > lower && value < higher );
        }
    }
}