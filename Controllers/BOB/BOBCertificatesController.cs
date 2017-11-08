using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BobService;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class BOBCertificatesController : BaseController
    {
        private readonly IBobService _service;
        private readonly IMapper _mapper;

        public BOBCertificatesController(IServiceFactory<IBobService> bobServiceFactory,
                                         IMapper mapper)
        {
            _service = bobServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/certificates")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetCertificatesAsync(new GetCertificatesRequest() { SiNumber = sinumber});
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0) 
                return BadRequest(response.BusinessMessages);
            var mapped = _mapper.Map<IEnumerable<BobCertificate>>(response.Value?.Certificates);
            return Ok(mapped);
        }
    }
}