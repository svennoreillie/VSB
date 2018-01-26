using VSBaseAngular.Business.ReturnServices;
using System;
using System.Collections.Generic;
using System.Linq;
using VSBaseAngular.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VSBaseAngular.Business
{
    public class ReturnPropositionService : IReturnPropositionService
    {

        private IPaymentLineService _paymentLineService;
        private IReturnValidationService _validation;
        private IEnumerable<IReturnItemService> _returnItemServices { get; set; }

        public ReturnPropositionService(IReturnValidationService validation,
                                        IPaymentLineService paymentLineService,
                                        IServiceProvider serviceProvider)
        {

            _validation = validation;
            _paymentLineService = paymentLineService;
            
            _returnItemServices = serviceProvider.GetServices<IReturnItemService>();
        }


        public async Task<decimal> CalculateMaximumDeductableAmount(long siNumber, string insz)
        {
            if (siNumber <= 0 || siNumber >= 99999999999) throw new ArgumentException("SiNumber is not within a valid range");

            var services = _returnItemServices.OrderBy(ris => ris.Kind);
            decimal sum = 0;
            foreach (var service in services)
            {
                sum += await service.GetMaxAmountReturnableAsync(siNumber, insz);
            }
            return sum;
        }

        public async Task<(List<ReturnCalculationResultLine> lines, decimal amount)> DistributeAmount(decimal amount, long siNumber, string insz)
        {
            List<ReturnCalculationResultLine> lines = new List<ReturnCalculationResultLine>();
            if (await CalculateMaximumDeductableAmount(siNumber, insz) == 0) return (lines, amount);

            foreach (var service in _returnItemServices.OrderBy(ris => ris.Kind))
            {
                if (amount <= 0) break;

                decimal maxAmount = await service.GetMaxAmountReturnableAsync(siNumber, insz);
                if (maxAmount == 0) continue;

                decimal lineAmount = Math.Min(maxAmount, amount);

                var line = new ReturnCalculationResultLine(service.Kind);
                line.Amount = lineAmount;
                lines.Add(line);

                amount -= lineAmount;
            }

            if (amount > 0 && lines.Count > 0)
            {
                //Distribute evenly over all possible lines
                decimal extra = amount / lines.Count;
                decimal rest = amount % lines.Count;
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].Amount += extra;
                    if (i == 0) lines[i].Amount += rest;
                }
            }

            return (lines, amount);
        }


        public async Task<Result<ReturnCalculationResponse>> CalculateDefaultProposition(ReturnCalculationRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (request.SiNumber <= 0 || request.SiNumber >= 99999999999) throw new ArgumentException("SiNumber is not within a valid range");

            var result = _validation.Validate(request);
            if (result.Code != ResultCode.Ok)
            {
                return new Result<ReturnCalculationResponse>() { Code = result.Code, Messages = result.Messages };
            }


            ReturnCalculationResponse response = new ReturnCalculationResponse();
            if (request.ReturnLines == null || request.ReturnLines.Count() == 0) return new Result<ReturnCalculationResponse>(response);

            response.AmountNonRefundable = _paymentLineService.GetUnreturnableAmount(request.IsFraude, request.ReturnLines);
            response.AmountRefundableByFOD = _paymentLineService.GetReturnableAmountByFOD(request.IsFraude, request.ReturnLines);
            decimal amountToRecover = _paymentLineService.GetReturnableAmount(request.IsFraude, request.ReturnLines);

            var distributeResponse = await this.DistributeAmount(amountToRecover, request.SiNumber, request.Insz);
            response.ReturnLines = distributeResponse.lines;
            response.AmountRefundableByOGM = distributeResponse.amount;

            return new Result<ReturnCalculationResponse>(response);
        }
    }
}