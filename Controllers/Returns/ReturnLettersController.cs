using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Business.ReturnServices;
using VSBaseAngular.Models;
using VsbService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ReturnLettersController : BaseController
    {

        private IReturnCalculationDataService _dataService;
        private readonly IHostingEnvironment _environment;

        public ReturnLettersController(IReturnCalculationDataService dataService,
                                       IHostingEnvironment env)
        {
            _dataService = dataService;
            _environment = env;

        }

        [HttpPost]
        public async Task<IActionResult> PostCreateLetter(ReturnCalculationRequest request)
        {
            if (ModelState.IsValid)
            {
                var originalFile = Path.Combine(_environment.ContentRootPath, "Docs", "Returns", "Regular.dotx");
                var newFile = Path.GetTempFileName();

                System.IO.File.Copy(originalFile, newFile, true);

                var dict = new Dictionary<string, string>
                            {
                                {"SiNumber", request.SiNumber.ToString()},
                                {"INSZ", request.Insz},
                                {"Reason", request.Reason},
                                {"Remark", request.Remark},
                            };

                using (var doc = WordprocessingDocument.Open(newFile, true))
                {
                    doc.ChangeDocumentType(WordprocessingDocumentType.Document);
                    var mainPart = doc.MainDocumentPart;
                    DocumentCoreGenerator.RunElements(mainPart.Document.Body, dict);
                    mainPart.Document.Save();
                    doc.Close();
                }

                var filename = string.Format("Brief terugvordering_{0}.docx", request.Insz);
                var result = await _dataService.StoreReturnCalculationAsync(request);

                string applicationType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                HttpContext.Response.ContentType = applicationType;
                var bytes = await System.IO.File.ReadAllBytesAsync(newFile);
                return new FileContentResult(bytes, applicationType) { FileDownloadName = filename };

            }
            return BadRequest(ModelState);
        }

        [Route("signed")]
        [HttpPost]
        public async Task<IActionResult> PostCreateSignedLetter(ReturnCalculationRequest request)
        {
            if (ModelState.IsValid)
            {
                var originalFile = Path.Combine(_environment.ContentRootPath, "Docs", "Returns", "SignedVersion.dotx");
                var newFile = Path.GetTempFileName();

                System.IO.File.Copy(originalFile, newFile, true);

                var dict = new Dictionary<string, string>
                            {
                                {"SiNumber", request.SiNumber.ToString()},
                                {"INSZ", request.Insz},
                                {"Reason", request.Reason},
                                {"Remark", request.Remark},
                            };

                using (var doc = WordprocessingDocument.Open(newFile, true))
                {
                    doc.ChangeDocumentType(WordprocessingDocumentType.Document);
                    var mainPart = doc.MainDocumentPart;
                    DocumentCoreGenerator.RunElements(mainPart.Document.Body, dict);
                    mainPart.Document.Save();
                    doc.Close();
                }
                var filename = string.Format("Aangetekende brief terugvordering_{0}.docx", request.Insz);
                var result = await _dataService.StoreReturnCalculationAsync(request);

                string applicationType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                HttpContext.Response.ContentType = applicationType;
                var bytes = await System.IO.File.ReadAllBytesAsync(newFile);
                return new FileContentResult(bytes, applicationType) { FileDownloadName = filename };
            }
            return BadRequest(ModelState);
        }

    }
}
