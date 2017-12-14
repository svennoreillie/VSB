using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DocumentService;
using System.Linq;
using VSBaseAngular.Business;
using VSBaseAngular.Models.Keys;
using ThabService;
using VSBaseAngular.Controllers;
using System.Net.Http;
using Microsoft.Extensions.Options;
using VSBaseAngular.Helpers.Options;
using System.IO;
using System.Web;

[ApiVersion(ControllerVersion.v1)]
[Route("api/v{version:apiVersion}/[Controller]")]
public class THABCalculationDocumentsController : BaseController
{
    private readonly IDocumentServiceVSB _documentService;
    private readonly IThabService _thabService;
    private readonly IReader<ThabCertificate> _thabCertificateReader;
    private HttpClient _client;
    private readonly IOptions<ApiConfig> _apiConfigs;

    public THABCalculationDocumentsController(IServiceFactory<IDocumentServiceVSB> documentServiceFabric, 
        IReader<ThabCertificate> thabCertificateReader, 
        IServiceFactory<IThabService> thabServiceFactory,
        HttpClient client,
        IOptions<ApiConfig> apiConfigs)
    {
        _documentService = documentServiceFabric.GetService();
        _thabService = thabServiceFactory.GetService();
        _thabCertificateReader = thabCertificateReader;
        _client = client;
        _apiConfigs = apiConfigs;
    }

    [Route("{sinumber:long}")]
    public async Task<IActionResult> Get(long sinumber, [FromQuery]string certificateId, [FromQuery]string insz)
    {
        var thabData = await _thabService.GetThabDataAsync(new GetThabDataRequest { Insz = insz, SiNumber = sinumber});
        var document = thabData?.Value?.ThabData?.FirstOrDefault(c => c.Certificate.Id == certificateId)?.Certificate?.CalculationBasicRightDocument;
        if (document == null) return NotFound();

        var fileName = string.Format($"ThabCalculation_{sinumber}.pdf");

        var section = _apiConfigs.Value?.Configs?.FirstOrDefault(a => a.ApplicationName == "IrisDocuments");
        if (section == null) throw new Exception("No configuration found for Popu Api");

        var url = Path.Combine(section.Url, sinumber.ToString());
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["reference"] = document.Reference;
        query["uniqueid"] = document.UniqueId;
        uriBuilder.Query = query.ToString();


        string applicationType = "application/pdf";
        HttpContext.Response.ContentType = applicationType;
        var bytes = await _client.GetByteArrayAsync(uriBuilder.ToString());
        FileContentResult result = new FileContentResult(bytes, applicationType) { FileDownloadName = fileName };
        return result;
    }
}