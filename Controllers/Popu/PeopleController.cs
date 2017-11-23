using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobService;
using Microsoft.AspNetCore.Mvc;
using ThabService;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;
using VSBaseAngular.Models.Search;
using ZvzService;

namespace VSBaseAngular.Controllers
{
    [ApiVersion(ControllerVersion.v1)]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class PeopleController : BaseController
    {
        private readonly IReader<Models.Person> _peopleReader;
        private IThabService _thabService;
        private IBobService _bobService;
        private readonly IZvzService _zvzService;

        public PeopleController(IReader<Models.Person> peopleReader,
                IServiceFactory<IThabService> thabFactory,
                IServiceFactory<IBobService> bobFactory,
                IServiceFactory<IZvzService> zvzFactory)
        {
            _peopleReader = peopleReader;
            _thabService = thabFactory.GetService();
            _bobService = bobFactory.GetService();
            _zvzService = zvzFactory.GetService();
        }


        [HttpGet]
        [Route("{sinumber:long}")]
        public async Task<IActionResult> Get(long sinumber)
        {
            var key = new PersonKey(sinumber);
            Models.Person p = await _peopleReader.GetAsync(key);
            return Ok(p);
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery]string firstname,
                                    [FromQuery]string name,
                                    //insz
                                    [FromQuery]string insz,
                                    //member
                                    [FromQuery]int? federation,
                                    [FromQuery]string membernr,
                                    //sinumber
                                    [FromQuery]long? sinumber,
                                    //state
                                    [FromQuery]string pillar,
                                    [FromQuery]bool? StateInitiated,
                                    [FromQuery]bool? StateCompleted,
                                    [FromQuery]DateTime? StateCompletedDate,
                                    [FromQuery]bool? StateRejected,
                                    [FromQuery]DateTime? StateRejectedDate,
                                    //Output
                                    [FromQuery]bool? CSV,
                                    //general
                                    [FromQuery]int skip,
                                    [FromQuery]int limit = int.MaxValue)
        {
            PersonSearch model = new PersonSearch();
            model.Federation = federation;
            model.FirstName = firstname;
            model.Insz = insz;
            model.MemberNr = membernr;
            model.Name = name;
            model.SiNumber = sinumber;

            model.Pillar = pillar;
            model.StateInitiated = StateInitiated;
            model.StateCompleted = StateCompleted;
            model.StateCompletedDate = StateCompletedDate;
            model.StateRejected = StateRejected;
            model.StateRejectedDate = StateRejectedDate;

            if (!CSV.HasValue || !CSV.Value)
            {
                model.Limit = limit;
                model.Skip = skip;
            }

            var people = await _peopleReader.SearchAsync(model);

            if (CSV.HasValue && CSV.Value)
            {
                //Output csv 
                return await this.GetPersonsCsv(people.Select(p => p.SiNumber));
            }

            return Ok(people.Where(p => p != null));
        }

        public async Task<IActionResult> GetPersonsCsv(IEnumerable<long> siNumbers)
        {
            var localFileName = string.Format("Opvraging lijst personen VSB_{0}.csv", Guid.NewGuid());
            var fileName = "Opvraging lijst personen VSB.csv";
            var newFile = Path.Combine(Path.GetTempPath(), localFileName);
            var delimiter1 = ";";
            var space = " ";

            var people = await _peopleReader.SearchAsync(new PersonSearch { SiNumbers = siNumbers });

            using (var writer = new StreamWriter(newFile, true))
            {
                foreach (var p in people)
                {
                    await WriteCSVLine(delimiter1, space, writer, p);
                }

                writer.Close();

                string applicationType = "text/csv";
                HttpContext.Response.ContentType = applicationType;
                var bytes = await System.IO.File.ReadAllBytesAsync(newFile);
                FileContentResult result = new FileContentResult(bytes, applicationType) { FileDownloadName = fileName };
                return result;
            }
        }

        private async Task WriteCSVLine(string delimiter1, string space, StreamWriter writer, Models.Person person)
        {
            var thabDataResponseTask = _thabService.GetThabDataAsync(new ThabService.GetThabDataRequest() { SiNumber = person.SiNumber, Insz = person.Insz });
            var bobDataResponseTask = _bobService.GetCertificatesAsync(new BobService.GetCertificatesRequest() { SiNumber = person.SiNumber });
            var zvzDataResponseTask = _zvzService.GetWarrantiesAsync(new GetWarrantiesRequest() { SiNumber = person.SiNumber });

            var lineSb = new StringBuilder();
            lineSb.Append(person.FederationNumber);
            lineSb.Append(delimiter1);
            lineSb.Append(person.Insz);
            lineSb.Append(delimiter1);
            lineSb.Append(person.FirstName);
            lineSb.Append(delimiter1);
            lineSb.Append(person.Name);
            lineSb.Append(delimiter1);
            lineSb.Append(person.Street);
            lineSb.Append(delimiter1);
            lineSb.Append(person.HouseNumber);
            lineSb.Append(delimiter1);
            lineSb.Append(person.NumberBox);
            lineSb.Append(delimiter1);
            lineSb.Append(person.ZipCode);
            lineSb.Append(delimiter1);
            lineSb.Append(person.Locality);
            lineSb.Append(delimiter1);

            var contactDetailSb = new StringBuilder();
            var delimiter2 = " - ";


            try
            {
                var zvzDataResponse = await zvzDataResponseTask;
                if (zvzDataResponse?.Value?.Warranties?.Any() == true)
                {
                    contactDetailSb.Append("ZVZ: ");
                    contactDetailSb.Append(zvzDataResponse?.Value?.Warranties?.FirstOrDefault()?.Care);
                    contactDetailSb.Append(delimiter2);
                    contactDetailSb.Append(zvzDataResponse?.Value?.Warranties?.FirstOrDefault()?.State);
                }
            }
            catch { }

            try
            {
                var bobDataResponse = await bobDataResponseTask;
                if (bobDataResponse?.Value?.Certificates?.Any() == true)
                {
                    if (contactDetailSb.Length != 0) contactDetailSb.Append(delimiter2);
                    contactDetailSb.Append("BOB - ");
                    contactDetailSb.Append(bobDataResponse?.Value?.Certificates?.FirstOrDefault()?.State);
                }
            }
            catch { }

            try
            {
                var thabDataResponse = await thabDataResponseTask;
                if (thabDataResponse?.Value?.ThabData?.FirstOrDefault()?.Certificate != null)
                {
                    if (contactDetailSb.Length != 0) contactDetailSb.Append(delimiter2);
                    contactDetailSb.Append("Thab - ");
                    contactDetailSb.Append(thabDataResponse?.Value?.ThabData?.First()?.Certificate?.State);
                }
            }
            catch { }

            lineSb.Append(contactDetailSb.ToString());
            lineSb.Append(delimiter1);

            StringBuilder thabDetailSb = null;
            try
            {
                var thabDataResponse = await thabDataResponseTask;
                if (thabDataResponse?.Value?.ThabData?.Any() == true)
                {
                    foreach (var data in thabDataResponse.Value.ThabData)
                    {
                        thabDetailSb = new StringBuilder(data.Certificate.Id);
                        thabDetailSb.Append(space);

                        if (data.CertificateNotifications != null && data.CertificateNotifications.notificications != null && data.CertificateNotifications.notificications.Any())
                        {
                            thabDetailSb.Append(data.CertificateNotifications.notificications.First().CreationDate.ToString("dd-MM-yyyy"));
                            thabDetailSb.Append(space);
                        }

                        if (data.Remark != null)
                            thabDetailSb.Append(data.Remark.TrimEnd());

                    }
                }
            }
            catch { }

            lineSb.Append(delimiter1);
            if (thabDetailSb != null) lineSb.Append(thabDetailSb.ToString());
            writer.WriteLine(lineSb.ToString());

        }
    }
}
