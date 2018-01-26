using AutoMapper;
using VSBaseAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VsbService;

namespace VSBaseAngular.Business.ReturnServices {
    public class ReturnCalculationDataService : IReturnCalculationDataService {
        private  IVsbService _vsbService;
        private  IReturnValidationService _validationService;
        private  IReturnPropositionService _propositionService;

        public ReturnCalculationDataService(IVsbService vsbService, 
            IReturnValidationService validationService,
            IReturnPropositionService propositionService) {
            _vsbService = vsbService;
            _validationService = validationService;
            _propositionService = propositionService;
        }

        public async Task<Result<GetReturnCalculationResponse>> GetReturnCalculationAsync(string SiNumber) {
            if (string.IsNullOrWhiteSpace(SiNumber)) throw new ArgumentException("SiNumber");
            long snr;
            if (long.TryParse(SiNumber, out snr)) {
                var returnMessage = await _vsbService.GetReturnCalculationAsync(snr);
                if (returnMessage.Value != null) return new Result<GetReturnCalculationResponse>(returnMessage.Value);
                return new Result<GetReturnCalculationResponse>() { Code = ResultCode.Error, Messages = returnMessage.BusinessMessages.Select(m => m.MessageString).ToList() };
            } else {
                throw new ArgumentOutOfRangeException("SiNumber is not a valid number");
            }
        }

        public async Task<Result<bool>> StoreReturnCalculationAsync(ReturnCalculationRequest request) {
            var validationResult = _validationService.Validate(request);
            if (validationResult.Code != ResultCode.Ok) return validationResult;

            var serviceRequest = Mapper.Map<SaveReturnCalculationRequest>(request);

            decimal amount = request.ReturnByDeduction;
            if (amount > 0) {
                var result = await _propositionService.DistributeAmount(amount, request.SiNumber, request.Insz);
                var lines = result.lines;
                amount = result.amount;

                if (amount > 0) return new Result<bool>(false) { Code = ResultCode.Error };
                foreach (var item in lines) {
                    switch (item.Kind) {
                        case VSBaseAngular.Models.ReturnCalculationKind.ZVZ:
                            serviceRequest.ReturnByZVZ = item.Amount;
                            break;
                        case VSBaseAngular.Models.ReturnCalculationKind.BOB:
                            serviceRequest.ReturnByBOB = item.Amount;
                            break;
                        case VSBaseAngular.Models.ReturnCalculationKind.THAB:
                            serviceRequest.ReturnByTHAB = item.Amount;
                            break;
                        default:
                            break;
                    }
                }

            }


            var returnMessage = await _vsbService.UpsertReturnCalculationAsync(serviceRequest);

            if (returnMessage.Value != null) {
                return new Result<bool>(returnMessage.Value.Succeeded);
            } else {
                return new Result<bool>(false) { Code = ResultCode.Error, Messages = returnMessage.BusinessMessages.Select(m => m.MessageString).ToList() };
            }
        }
                
    }
}