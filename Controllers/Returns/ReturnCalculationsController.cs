using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VsbService;
using Microsoft.AspNetCore.Authorization;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Authorize]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ReturnCalculationsController : BaseController
    {
        private IReturnPropositionService _service;

        public ReturnCalculationsController(IReturnPropositionService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> PostReturnLines([FromBody]ReturnCalculationRequest request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedBy = User.Identity.Name;
                var response = await _service.CalculateDefaultProposition(request);
                switch (response.Code)
                {
                    case ResultCode.Ok:
                        return Ok(response.Value);
                    case ResultCode.Error:
                        return BadRequest(string.Join(";", response.Messages));
                    case ResultCode.Warning:
                        return Ok(response.Value);
                    default:
                        break;
                }
            }
            return BadRequest(ModelState);
        }
    }
}
