using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ThabService;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;
using System;
using System.Linq;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABPaymentsController : BaseController
    {
        private readonly IReader<ThabCertificate> _thabcertificateService;
        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABPaymentsController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<ThabCertificate> thabcertificateService,
                                         IMapper mapper)
        {
            _thabcertificateService = thabcertificateService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/payments")]
        public async Task<IActionResult> Get(long sinumber, [FromQuery]string insz)
        {
            var certificates = await _thabcertificateService.SearchAsync(new ThabCertificateSearch { SiNumber = sinumber, Insz = insz });

            List<ThabPayment> payments = new List<ThabPayment>();

            foreach (var cert in certificates)
            {
                var request = new GetPaymentsRequest()
                {
                    SiNumber = sinumber,
                    ReferenceDate = cert.ReferenceDate
                };
                var response = await _service.GetPaymentsAsync(request);
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);

                var mappedModels = _mapper.Map<IEnumerable<ThabPayment>>(response?.Value?.Payments, opt => opt.Items["Id"] = cert.CertificateId);
                payments.AddRange(mappedModels);
            }

            return Ok(payments);
        }

        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/fodpayments")]
        public async Task<IActionResult> GetFOD(long sinumber, [FromQuery]string insz)
        {
            var certificates = await _thabcertificateService.SearchAsync(new ThabCertificateSearch { SiNumber = sinumber, Insz = insz });

            List<ThabPayment> payments = new List<ThabPayment>();

            foreach (var cert in certificates.Where(c => c.IsMigrated && c.DecisionDate.HasValue && c.BeginDate.HasValue))
            {
                int diff = GetMonthDifference(cert.BeginDate.Value, cert.DecisionDate.Value);
                diff++; //Decisionmonth also payed by FOD
                //amount
                var amountResponse = await _service.GetCertificatesPayableAmountsAsync(new GetCertificatesPayableAmountRequest()
                {
                    SiNumber = sinumber,
                    Id = cert.CertificateId
                });

                for (int i = 0; i < diff; i++)
                {
                    DateTime beginDate = cert.BeginDate.Value.AddMonths(i);
                    decimal? amount = amountResponse.Value?.PayableAmounts?.OrderByDescending(pa => pa.Start)?
                                                                           .FirstOrDefault(pa => pa.Start <= beginDate && (!pa.End.HasValue || beginDate <= pa.End.Value))?
                                                                           .Amount;
                    payments.Add(new ThabPayment()
                    {
                        BeginDate = beginDate,
                        EndDate = beginDate.AddMonths(1).AddDays(-1),
                        Amount = amount ?? 0,
                        CertificateId = cert.CertificateId,
                        Currency = "EUR"
                    });
                }
            }

            return Ok(payments);
        }

        public int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}
