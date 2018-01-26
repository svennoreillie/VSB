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
        [Route("{sinumber}")]
        public async Task<IActionResult> Get(string sinumber)
        {
            long snr;
            if (!long.TryParse(sinumber, out snr)) return BadRequest();
            var response = await _vsbService.GetRemarkAsync(new GetRemarkRequest() { Sinr = snr });
            if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                return BadRequest(response.BusinessMessages);

            return Ok(response.Value?.Remark);
        }

        [HttpPost]
        [Route("{sinumber}")]
        public async Task<IActionResult> Post(string sinumber, [FromBody]PostRemarkModel model)
        {
            long snr;
            if (!long.TryParse(sinumber, out snr)) return BadRequest();
            if (ModelState.IsValid)
            {
                var response = await _vsbService.SaveRemarkAsync(new SaveRemarkRequest() { Sinr = snr, Remark = model.Remark, Userid = GetUserID() });
                if (response.BusinessMessages != null && response.BusinessMessages.Length > 0)
                    return BadRequest(response.BusinessMessages);

                return Ok(response.Value?.saved);
            }
            return BadRequest(ModelState);
        }

        private string GetUserID()
        {
            string user = User.Identity.Name;
            return user.Substring(user.IndexOf('\\') + 1);
        }

    }
}
