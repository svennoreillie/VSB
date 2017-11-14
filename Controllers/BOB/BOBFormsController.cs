using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BobService;
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

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class BOBFormsController : BaseController
    {
        private readonly IBobService _bobService;
        private IMapper _mapper;
        private readonly IHostingEnvironment _environment;

        public BOBFormsController(IServiceFactory<IBobService> bobServiceFactory, IMapper mapper, IHostingEnvironment env)
        {
            _bobService = bobServiceFactory.GetService();
            _mapper = mapper;
            _environment = env;
        }


        [Route("~/api/v{version:apiVersion}/bobpeople/{sinumber:long}/form")]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            Document document = null;
            FileStream fs = null;
            PdfReader reader = null;
            PdfWriter writer = null;

            var fileName = "aanvraag formulier bob";
            var theName = string.Empty;
            var theStreetAndNb = string.Empty;
            var thePostalAndLocality = string.Empty;
            var theCountry = string.Empty;
            var theSex = -1;
            var theNationality = string.Empty;
            DateTime? theBirthDate = null;
            var theTelNb = string.Empty;
            var theEmail = string.Empty;
            var theNissNb = string.Empty;
            var theIban = string.Empty;
            var theBic = string.Empty;

            var getPersonResponse = await _bobService.GetPersonAsync(new GetPersonRequest { SiNumber = sinumber });
            if (getPersonResponse?.BusinessMessages.Any() == true) return BadRequest(getPersonResponse.BusinessMessages);

            var person = getPersonResponse?.Value?.person;
            if (person == null) return BadRequest("Person not filled");

            theName = string.Format("{0} {1}", person.FirstName, person.Name);
            fileName = string.Format("{0} voor {1}.pdf", fileName, theName);
            theSex = person.Sex;
            theNationality = person.Nationality;
            if (person.contactData != null)
            {
                if (!string.IsNullOrWhiteSpace(person.contactData.PhoneNumber))
                {
                    theTelNb = person.contactData.PhoneNumber;
                }
                if (!string.IsNullOrWhiteSpace(person.contactData.Email))
                {
                    theEmail = person.contactData.Email;
                }
            }
            theBirthDate = person.BirthDate;
            theNissNb = person.Insz;
            if (person.CurrentBOBAccountNr != null)
            {
                theIban = person.CurrentBOBAccountNr.Iban;
                theBic = person.CurrentBOBAccountNr.Bic;
            }

            if (person.mainAddress != null)
            {
                var mainAddress = person.mainAddress;
                theStreetAndNb = string.Format("{0} {1} {2}", mainAddress.Street, mainAddress.HouseNumber, mainAddress.NumberBox);
                thePostalAndLocality = string.Format("{0} {1}", mainAddress.ZipCode, mainAddress.Locality);
                theCountry = mainAddress.Country;
            }


            var originalFile = Path.Combine(_environment.ContentRootPath, "Docs/Bob/Bob_form.pdf");
            var newFile = Path.GetTempFileName();
            newFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(newFile) + ".pdf");
            reader = new PdfReader(originalFile);
            var size = reader.GetPageSizeWithRotation(1);
            document = new Document(size);
            fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            var cb = writer.DirectContent;
            var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.Black);
            cb.SetFontAndSize(bf, 11);
            cb.BeginText();
            cb.ShowTextAligned(0, theName, 191, 438, 0);
            cb.ShowTextAligned(0, theStreetAndNb, 191, 418, 0);
            cb.ShowTextAligned(0, thePostalAndLocality, 191, 398, 0);
            cb.ShowTextAligned(0, theCountry, 191, 378, 0);
            if (theSex > -1)
            {
                cb.ShowTextAligned(0, "X", theSex % 2 == 1 ? 196 : 303, 357, 0);
            }
            cb.ShowTextAligned(0, theNationality, 191, 338, 0);
            cb.ShowTextAligned(0, theTelNb, 191, 318, 0);
            cb.ShowTextAligned(0, theEmail, 191, 298, 0);
            if (theBirthDate.HasValue)
            {
                cb.ShowTextAligned(0, theBirthDate.Value.Day.ToString("d2"), 224, 278, 0);
                cb.ShowTextAligned(0, theBirthDate.Value.Month.ToString("d2"), 281, 278, 0);
                cb.ShowTextAligned(0, theBirthDate.Value.Year.ToString("d4"), 330, 278, 0);
            }
            if (theNissNb.Length == 11)
            {
                cb.ShowTextAligned(0, theNissNb.Substring(0, 6), 191, 258, 0);
                cb.ShowTextAligned(0, theNissNb.Substring(6, 3), 244, 258, 0);
                cb.ShowTextAligned(0, theNissNb.Substring(9, 2), 281, 258, 0);
            }
            if (theIban.Length == 16)
            {
                cb.ShowTextAligned(0, theIban.Substring(0, 4), 191, 238, 0);
                cb.ShowTextAligned(0, theIban.Substring(4, 4), 228, 238, 0);
                cb.ShowTextAligned(0, theIban.Substring(8, 4), 264, 238, 0);
                cb.ShowTextAligned(0, theIban.Substring(12, 4), 299, 238, 0);
            }
            cb.ShowTextAligned(0, theBic, 191, 218, 0);
            cb.EndText();
            var page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);

            for (int p = 2; p <= reader.NumberOfPages; p++)
            {
                document.SetPageSize(reader.GetPageSizeWithRotation(1));
                document.NewPage();
                page = writer.GetImportedPage(reader, p);
                cb.AddTemplate(page, 0, 0);
            }

            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();


            string applicationType = "application/pdf";
            HttpContext.Response.ContentType = applicationType;
            var bytes = await System.IO.File.ReadAllBytesAsync(newFile);
            FileContentResult result = new FileContentResult(bytes, applicationType) { FileDownloadName = fileName };
            return result;
        }
    }
}