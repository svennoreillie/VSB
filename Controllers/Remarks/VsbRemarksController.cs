using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VsbService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class VsbRemarksController : BaseController
    {
        private readonly IVsbService _vsbService;

        public VsbRemarksController(IServiceFactory<IVsbService> vsbServiceFactory)
        {
            _vsbService = vsbServiceFactory.GetService();
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var response = await _vsbService.GetRemarkAsync(new GetRemarkRequest() { Sinr = sinumber });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            return Ok(response.Value?.Remark);
        }

        [HttpPost]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Post(long sinumber, PostRemarkModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _vsbService.SaveRemarkAsync(new SaveRemarkRequest() { Sinr = sinumber, Remark = model.Remark });
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);

                return Ok(response.Value?.saved);
            }
            return BadRequest(ModelState);
        }

    }
}
