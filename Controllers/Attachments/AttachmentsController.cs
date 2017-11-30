using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using VSBaseAngular.Controllers;

[ApiVersion(ControllerVersion.v1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AttachmentsController : BaseController
{
    [HttpGet]
    [Route("{sinumber:long}")]
    public async Task<IActionResult> GetAttachments(string sinumber)
    {
        try
        {
            if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");

            var clientAttachmentsFolder = GetClientAttachmentsFolder(sinumber);
            if (!Directory.Exists(clientAttachmentsFolder)) return Ok(new List<VSBAttachment>());

            string[] files = Directory.GetFiles(clientAttachmentsFolder);

            var attachments = ProjectAttachments(sinumber, files);

            return Ok(attachments);
        }
        catch (DirectoryNotFoundException)
        {
            return Ok();
        }
    }

    [HttpGet]
    [Route("{sinumber:long}")]

    public async Task<IActionResult> GetAttachment(string sinumber, [FromQuery]string filename)
    {
        try
        {
            if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");

            var clientAttachmentsFolder = GetClientAttachmentsFolder(sinumber);
            string[] files = Directory.GetFiles(clientAttachmentsFolder);

            var attachments = ProjectAttachments(sinumber, files);
            var file = attachments.FirstOrDefault(a => a.Filename == filename);

            if (file == null) return NotFound();

            string fullFileName = Path.GetFileName(file.FilePath);

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fullFileName, out contentType);
            contentType = contentType ?? "application/octet-stream";

            string applicationType = "application/pdf";
            HttpContext.Response.ContentType = applicationType;
            var bytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
            FileContentResult result = new FileContentResult(bytes, applicationType) { FileDownloadName = fullFileName };
            return result;
        }
        catch (DirectoryNotFoundException)
        {
            return Ok();
        }
    }

    [HttpDelete]
    [Route("{sinumber}")]
    public async Task<IActionResult> Delete(string sinumber, [FromQuery]string username, [FromQuery]string filename)
    {
        try
        {
            if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");

            var clientAttachmentsFolder = GetClientAttachmentsFolder(sinumber);
            string[] files = Directory.GetFiles(clientAttachmentsFolder);

            var attachments = ProjectAttachments(sinumber, files);
            var file = attachments.FirstOrDefault(a => a.Filename == filename);

            if (file == null) return NotFound();
            if (!file.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)) return Unauthorized();

            System.IO.File.Delete(file.FilePath);

            return Ok();

        }
        catch (DirectoryNotFoundException)
        {
            return Ok(new List<VSBAttachment>());
        }
    }

    [HttpPost]
    [Route("{sinumber}")]
    public async Task<IActionResult> Post(string sinumber, [FromQuery]string username, [FromForm]IFormFile file)
    {
        if (file == null) throw new Exception("File is null");
        if (file.Length == 0) throw new Exception("File is empty");

        if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");
        if (string.IsNullOrEmpty(username)) return BadRequest("Username was not provided");

        try
        {
            var clientFolder = GetClientAttachmentsFolder(sinumber);
            if (!Directory.Exists(clientFolder)) Directory.CreateDirectory(clientFolder);

            string originalFilename = file.FileName.Replace("\"", string.Empty);
            string newFileName = string.Format("{0}_{1}", username, originalFilename);

            string path = Path.Combine(clientFolder, newFileName);

            if (newFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return BadRequest("Invalid filename");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new VSBAttachment());
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }



    private string GetClientAttachmentsFolder(string SiNumber)
    {
        //todo
        return "c:/";
        // if (string.IsNullOrWhiteSpace(SiNumber)) throw new ArgumentException("SiNumber");
        // var shareLocation = ConfigurationManager.AppSettings.Get("AttachmentShare");
        // var folderName = ConfigurationManager.AppSettings.Get("AttachmentFolder");
        // return Path.Combine(shareLocation, folderName, SiNumber);
    }

    private IEnumerable<VSBAttachment> ProjectAttachments(string sinumber, string[] files)
    {
        foreach (var file in files)
        {
            var fullFilename = Path.GetFileNameWithoutExtension(file);
            int underscorePosition = fullFilename.IndexOf('_');

            var username = fullFilename.Substring(0, underscorePosition);
            var filename = fullFilename.Substring(underscorePosition + 1);
            FileInfo info = new FileInfo(file);


            yield return new VSBAttachment
            {
                SiNumber = sinumber,
                Username = username,
                Filename = filename,
                FilePath = file,
                UploadDate = info.CreationTime
            };
        }
    }
}