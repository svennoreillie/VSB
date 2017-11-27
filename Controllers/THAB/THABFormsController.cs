using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ThabService;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class THABFormsController : BaseController
    {
        private readonly IThabService _thabService;
        private IMapper _mapper;
        private readonly IHostingEnvironment _environment;


        public THABFormsController(IServiceFactory<IThabService> thabServiceFactory,
            IMapper mapper,
            IHostingEnvironment env)
        {
            _thabService = thabServiceFactory.GetService();
            _mapper = mapper;
            _environment = env;
        }

        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber, [FromQuery]DateTime referenceDate)
        {
            var getPersonalDataResponse = await _thabService.GetPersonalDataAsync(new GetPersonalDataRequest { SiNumber = sinumber });

            if (getPersonalDataResponse?.Value?.PersonalData == null)
                throw new Exception($"Person with sinr {sinumber} not found");

            var pdata = getPersonalDataResponse.Value.PersonalData;
            var localFileName = string.Format("Opvragen gegevens THAB_{0}_{1}_{2}.docx", pdata.FirstNameMember, pdata.NameMember, Guid.NewGuid());
            var fileName = string.Format("Opvragen gegevens THAB_{0}_{1}.docx", pdata.FirstNameMember, pdata.NameMember);

            var docsPath = Path.Combine(_environment.ContentRootPath, "Docs", "Thab");

            string originalFile;
            if (pdata.Federation < 400)
            {
                switch (pdata.Federation)
                {
                    case (304):
                    case (309):
                    case (311):
                    case (322):
                        originalFile = Path.Combine(docsPath, $"Opvragen gegevens THAB {pdata.Federation}.dotx");
                        break;
                    default:
                        originalFile = Path.Combine(docsPath, "Opvragen gegevens THAB 300.dotx");
                        break;
                }
            }
            else originalFile = Path.Combine(docsPath, "Opvragen gegevens THAB 400.dotx");

            var newFile = Path.Combine(Path.GetTempPath(), localFileName);
            System.IO.File.Copy(originalFile, newFile);

            var street_nr = pdata.StreetContact;
            if (!string.IsNullOrWhiteSpace(pdata.HouseNumberContact))
                street_nr += " " + pdata.HouseNumberContact;
            if (!string.IsNullOrWhiteSpace(pdata.NumberBoxContact))
                street_nr += " " + pdata.NumberBoxContact;

            var dict = new Dictionary<string, string>
                {
                    {"A_Name", pdata.NameContact},
                    {"A_FirstName", pdata.FirstNameContact},
                    {"A_Street&Nr", street_nr},
                    {"A_ZipCode&City", pdata.ZipCodeContact + " " + pdata.CityContact},
                    {"A_Date", DateTime.Now.ToString("dd-MM-yyyy")},
                    {"B_Insz", pdata.Insz},
                    {"C_Title", pdata.GenderMember == 1 ? "heer" : "mevrouw"},
                    {"C_Name", pdata.NameMember},
                    {"C_ReferenceDate", referenceDate.ToString("dd-MM-yyyy")},
                    {"D_FreeField"," "}
                };

            using (var doc = WordprocessingDocument.Open(newFile, true))
            {
                doc.ChangeDocumentType(WordprocessingDocumentType.Document);
                var mainPart = doc.MainDocumentPart;
                DocumentCoreGenerator.RunElements(mainPart.Document.Body, dict);
                mainPart.Document.Save();
                doc.Close();
            }

            string applicationType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            HttpContext.Response.ContentType = applicationType;
            var bytes = await System.IO.File.ReadAllBytesAsync(newFile);
            FileContentResult result = new FileContentResult(bytes, applicationType) { FileDownloadName = fileName };
            return result;

        }
    }
}