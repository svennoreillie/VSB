using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThabService;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABLettersController : BaseController
    {
        private readonly IThabService _service;
        private readonly IMapper _mapper;

        public THABLettersController(IServiceFactory<IThabService> thabServiceFactory, IMapper mapper)
        {
            _service = thabServiceFactory.GetService();
            _mapper = mapper;
        }


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/thabcertificates/{certificateId}/leters")]
        public async Task<IActionResult> Get(string certificateId)
        {
            var response = await _service.GetLettersAsync(new GetLettersRequest() { CertificateId = certificateId });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);
            var mappedModels = _mapper.Map<IEnumerable<ThabLetter>>(response.Value?.Letters);
            return Ok(mappedModels);
        }

        [Route("{sinumber:long}")]
        public async Task<IActionResult> GetAll(long sinumber, [FromQuery]string insz)
        {
            var certificates = await _service.GetCertificatesAsync(new GetCertificatesRequest { SiNumber = sinumber, Insz = insz });

            List<Letter> letters = new List<Letter>();

            foreach (var certificate in certificates?.Value?.Certificates)
            {
                var response = await _service.GetLettersAsync(new GetLettersRequest() { CertificateId = certificate.Id });
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);
                letters.AddRange(_mapper.Map<IEnumerable<ThabLetter>>(response.Value?.Letters));
            }

            return Ok(letters);
        }
    }
}