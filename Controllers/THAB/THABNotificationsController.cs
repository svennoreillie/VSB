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
    public class THABNotificationsController : BaseController
    {
        private readonly IReader<Person> _peopleService;
        private readonly IReader<ThabCertificate> _thabCertificateService;
        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABNotificationsController(IServiceFactory<IThabService> thabServiceFactory,
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
        [Route("{certificateId}")]        
        [Route("~/api/v{version:apiVersion}/thabcertificates/{sinumber:long}/notifications/{certificateId}")]
        public async Task<IActionResult> GetAll(string certificateId)
        {
            var response = await _service.GetCertificateNotificationsAsync(new GetCertificateNotificationsRequest {
                CertificateId = certificateId
            });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);  

            var mappedModels = _mapper.Map<ThabNotification>(response.Value?.CertificateNotifications);
            return Ok(mappedModels);
        }
    }
}