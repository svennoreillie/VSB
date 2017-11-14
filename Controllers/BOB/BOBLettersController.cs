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
    public class BOBLettersController : BaseController
    {
        private readonly IBobService _service;
        private readonly IMapper _mapper;

        public BOBLettersController(IServiceFactory<IBobService> bobServiceFactory, IMapper mapper)
        {
            _service = bobServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/certificates/{certificateId}/leters")]
        [Route("~/api/v{version:apiVersion}/bobcertificates/{certificateId}/leters")]
        public async Task<IActionResult> Get(string certificateId)
        {
            var response = await _service.GetLettersAsync(new GetLettersRequest() { CertificateId = certificateId });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);
            var mappedModels = _mapper.Map<IEnumerable<BobLetter>>(response.Value?.Letters);
            return Ok(mappedModels);
        }

        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/letters")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber)
        {
            var certificates = await _service.GetCertificatesAsync(new GetCertificatesRequest { SiNumber = sinumber });

            List<BobLetter> letters = new List<BobLetter>();

            foreach (var certificate in certificates?.Value?.Certificates)
            {
                var response = await _service.GetLettersAsync(new GetLettersRequest() { CertificateId = certificate.Id });
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);
                letters.AddRange(_mapper.Map<IEnumerable<BobLetter>>(response.Value?.Letters));
            }

            return Ok(letters);
        }
    }
}