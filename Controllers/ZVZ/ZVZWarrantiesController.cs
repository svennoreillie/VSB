using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;
using ZvzService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ZVZWarrantiesController : BaseController
    {
        private readonly IZvzService _service;
        private IMapper _mapper;

        public ZVZWarrantiesController(IServiceFactory<IZvzService> zvzServiceFactory, IMapper mapper)
        {
            this._service = zvzServiceFactory.GetService();
            this._mapper = mapper;
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _service.GetWarrantiesAsync(new GetWarrantiesRequest() { SiNumber = sinumber });

            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            var source = response.Value?.Warranties?.OrderByDescending(w => w.DateFrom)
                                                    .ThenByDescending(w => w.RequestDate);

            var mappedModel = _mapper.Map<IEnumerable<ZvzWarranty>>(source);
            return Ok(mappedModel);
        }

    }
}