using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DocumentService;
using System.Linq;
using VSBaseAngular.Business;
using VSBaseAngular.Models.Keys;
using ThabService;
using VSBaseAngular.Controllers;

[ApiVersion(ControllerVersion.v1)]
[Route("api/v{version:apiVersion}/[Controller]")]
public class THABCalculationDocumentsController : BaseController
{
    private readonly IDocumentServiceVSB _documentService;
    private readonly IThabService _thabService;
    private readonly IReader<ThabCertificate> _thabCertificateReader;

    public THABCalculationDocumentsController(IServiceFactory<IDocumentServiceVSB> documentServiceFabric, 
        IReader<ThabCertificate> thabCertificateReader, 
        IServiceFactory<IThabService> thabServiceFactory)
    {
        _documentService = documentServiceFabric.GetService();
        _thabService = thabServiceFactory.GetService();
        _thabCertificateReader = thabCertificateReader;
    }

    [Route("{sinumber:long}")]
    public async Task<IActionResult> Get(long sinumber, [FromQuery]string certificateId, [FromQuery]string insz)
    {
        var thabData = await _thabService.GetThabDataAsync(new GetThabDataRequest { Insz = insz, SiNumber = sinumber});
        var document = thabData?.Value?.ThabData?.FirstOrDefault(c => c.Certificate.Id == certificateId)?.Certificate?.CalculationBasicRightDocument;
        if (document == null) return NotFound();

        var fileName = string.Format($"ThabCalculation_{sinumber}.pdf");

        var GetDocumentRequest = new GetIrisDocBinaryRequest() { memberNsi = sinumber.ToString(), reference = document.Reference, uniqueID = document.UniqueId };
        var GetDocumentResponse = await _documentService.GetIrisDocBinaryAsync(GetDocumentRequest);
        var documentLink = GetDocumentResponse.DocumentLinks.FirstOrDefault();

        if (documentLink == null || string.IsNullOrWhiteSpace(documentLink.docNum))
            throw new Exception("GetIrisDocBinary returns nothing.");

        if (!documentLink.irisDocBinaryLink.StartsWith("\\"))
            throw new Exception(string.Format("GetIrisDocBinary:{0}", documentLink.irisDocBinaryLink));

        var downloadIrisDocBinaryResponse = await _documentService.DownloadIrisDocBinaryAsync(
            new DownloadIrisDocBinaryRequest { docNum = documentLink.docNum, pathToTempFile = documentLink.irisDocBinaryLink });
            

        if (downloadIrisDocBinaryResponse.binaryType == "")
            throw new Exception(string.Format("DownloadIrisDocBinary:{0}", downloadIrisDocBinaryResponse.binaryFile));

        string applicationType = "application/pdf";
            HttpContext.Response.ContentType = applicationType;
            var bytes = await System.IO.File.ReadAllBytesAsync(downloadIrisDocBinaryResponse.binaryFile);
            FileContentResult result = new FileContentResult(bytes, applicationType) { FileDownloadName = fileName };
            return result;
    }
}