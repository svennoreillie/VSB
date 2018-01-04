using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Business.ReturnServices;
using VSBaseAngular.Models;
using VsbService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ReturnsController : BaseController
    {
        private IReturnCalculationDataService _dataService;

        public ReturnsController(IReturnCalculationDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string sinumber)
        {
            if (string.IsNullOrWhiteSpace(sinumber)) return BadRequest("SiNumber is not provided");
            var result = await _dataService.GetReturnCalculationAsync(sinumber);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ReturnCalculationRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _dataService.StoreReturnCalculationAsync(request);
                if (result.Code != ResultCode.Ok) return BadRequest(string.Join(";", result.Messages));

                return Ok();
            }
            return BadRequest(ModelState);
        }

    }
}
