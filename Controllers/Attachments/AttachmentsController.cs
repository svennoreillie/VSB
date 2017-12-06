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
using Microsoft.Extensions.Options;
using VSBaseAngular.Controllers;
using VSBaseAngular.Helpers.Options;
using VSBaseAngular.Business;
using VSBaseAngular.Models;
using VSBaseAngular.Models.Keys;

[ApiVersion(ControllerVersion.v1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AttachmentsController : BaseController
{
    private readonly IOptions<AppConfig> _config;
    private readonly IReader<User> _userReader;

    public AttachmentsController(IOptions<AppConfig> config, IReader<User> userReader)
    {
        _config = config;
        _userReader = userReader;
    }


    [HttpGet]
    [Route("{sinumber}")]
    public IActionResult GetAttachments(string sinumber)
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
    [Route("{sinumber}/{filename}")]

    public async Task<IActionResult> GetAttachment(string sinumber, string filename)
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
    [Route("{sinumber}/{filename}")]
    public async Task<IActionResult> Delete(string sinumber, string filename)
    {
        try
        {
            var username = await this.GetUserName();
            if (string.IsNullOrEmpty(username)) return BadRequest("username was not provided");
            if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");

            var clientAttachmentsFolder = GetClientAttachmentsFolder(sinumber);
            string[] files = Directory.GetFiles(clientAttachmentsFolder);

            var attachments = ProjectAttachments(sinumber, files);
            var file = attachments.FirstOrDefault(a => a.Filename == filename);

            if (file == null) return NotFound();
            if (!file.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)) return Unauthorized();

            System.IO.File.Delete(file.FilePath);

            return Ok(file);
        }
        catch (DirectoryNotFoundException)
        {
            return Ok(new List<VSBAttachment>());
        }
    }

    [HttpPost]
    [Route("{sinumber}")]
    public async Task<IActionResult> Post(string sinumber, [FromForm]IFormFile file)
    {
        if (file == null) throw new Exception("File is null");
        if (file.Length == 0) throw new Exception("File is empty");

        var username = await this.GetUserName();
        if (string.IsNullOrEmpty(username)) return BadRequest("Username was not provided");
        if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");

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

            return Ok(new VSBAttachment
            {
                SiNumber = sinumber,
                Username = username,
                Filename = newFileName,
                FilePath = path,
                UploadDate = DateTime.Now
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }



    private string GetClientAttachmentsFolder(string SiNumber)
    {
        if (string.IsNullOrWhiteSpace(SiNumber)) throw new ArgumentException("SiNumber");
        var shareLocation = _config.Value?.AttachmentShare;
        var folderName = _config.Value?.AttachmentFolder;
        return Path.Combine(shareLocation, folderName, SiNumber);
    }

    private async Task<string> GetUserName()
    {
        string user = User.Identity.Name;
        if (string.IsNullOrEmpty(user)) return null;

        var response = await _userReader.GetAsync(new UserKey(user.Split('\\')[0], user.Split('\\')[1]));
        return response.DisplayName;
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