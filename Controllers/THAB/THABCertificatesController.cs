using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ThabService;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABCertificatesController : BaseController
    {
        private readonly IReader<Person> _peopleService;
        private readonly IReader<ThabCertificate> _thabCertificateService;
        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABCertificatesController(IServiceFactory<IThabService> thabServiceFactory,
                                         IReader<ThabCertificate> thabCertificateService,
                                         IReader<Person> peopleService,
                                         IMapper mapper)
        {
            _peopleService = peopleService;
            _thabCertificateService = thabCertificateService;
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber, [FromQuery]string insz)
        {
            var certificates = await _thabCertificateService.SearchAsync(new ThabCertificateSearch() { SiNumber = sinumber, Insz = insz });
            foreach (var cert in certificates)
            {
                var remark = await _service.GetRemarkAsync(new GetRemarkRequest { SiNumber = sinumber, ReferenceDate = cert.ReferenceDate});
                cert.Remark = remark?.Value?.Remark;
                var notifications = await _service.GetCertificateNotificationsAsync(new GetCertificateNotificationsRequest() { CertificateId = cert.CertificateId });
                foreach (var notification in notifications?.Value?.CertificateNotifications?.notificications)
                {
                    cert.Tooltip += $"{notification.CreationDate.ToShortDateString()}: {notification.Description} \n";
                }
                cert.TooltipTile = notifications?.Value?.CertificateNotifications?.nextStep?.Description;
            }
            return Ok(certificates);

        
        }
    }
}