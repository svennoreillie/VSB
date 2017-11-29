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
    [Route("{sinumber:long}")]
    public async Task<IActionResult> Post(long sinumber, [FromForm]IFormFile file)
    {
        if (file == null) throw new Exception("File is null");
        if (file.Length == 0) throw new Exception("File is empty");

        var filePath = Path.GetTempFileName();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return Ok();

        //  using (Stream stream = file.OpenReadStream())
        //     {
        //         using (var binaryReader = new BinaryReader(stream))
        //         {
        //             var fileContent = binaryReader.ReadBytes((int)file.Length);
        //             await _uploadService.AddFile(fileContent, file.FileName, file.ContentType);
        //         }
        //     }





        //         long size = files.Sum(f => f.Length);

        //         // full path to file in temp location
        //         var filePath = Path.GetTempFileName();

        //         foreach (var formFile in files)
        //         {
        //             if (formFile.Length > 0)
        //             {
        //                 using (var stream = new FileStream(filePath, FileMode.Create))
        //                 {
        //                     await formFile.CopyToAsync(stream);
        //                 }
        //             }
        //         }

        //         // process uploaded files
        //         // Don't rely on or trust the FileName property without validation.

        //         return Ok(new { count = files.Count, size, filePath });
    }

    // [HttpPost]
    // public async Task<IHttpActionResult> PostAttachment([FromUri]string sinumber, [FromUri]string username)
    // {
    //     if (string.IsNullOrEmpty(sinumber)) return BadRequest("SiNumber was not provided");
    //     if (string.IsNullOrEmpty(username)) return BadRequest("Username was not provided");
    //     string filePath = null;
    //     try
    //     {
    //         if (!Request.Content.IsMimeMultipartContent())
    //         {
    //             this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
    //         }

    //         var provider = GetMultipartProvider();
    //         var result = await Request.Body.ReadAsMultipartAsync(provider);
    //         var clientFolder = GetClientAttachmentsFolder(sinumber);
    //         if (!Directory.Exists(clientFolder)) Directory.CreateDirectory(clientFolder);

    //         // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
    //         foreach (var file in result.FileData)
    //         {
    //             string originalFilename = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
    //             string newFileName = string.Format("{0}_{1}", username, originalFilename);

    //             string path = Path.Combine(clientFolder, newFileName);

    //             //Copy to attachmentShare
    //             File.Move(file.LocalFileName, path);
    //         }

    //         return Ok();
    //     }
    //     catch (UnauthorizedAccessException ex)
    //     {
    //         ex.LogToErrorLog();
    //         return Unauthorized();
    //     }
    //     finally
    //     {
    //         if (filePath != null) File.Delete(filePath);
    //     }
    // }


    private string GetClientAttachmentsFolder(string SiNumber)
    {
        //todo
        return "c:/";
        // if (string.IsNullOrWhiteSpace(SiNumber)) throw new ArgumentException("SiNumber");
        // var shareLocation = ConfigurationManager.AppSettings.Get("AttachmentShare");
        // var folderName = ConfigurationManager.AppSettings.Get("AttachmentFolder");
        // return Path.Combine(shareLocation, folderName, SiNumber);
    }

    // private MultipartFormDataStreamProvider GetMultipartProvider()
    // {
    //     var uploadFolder = "~/App_Data/Tmp/FileUploads";
    //     var root = System.Web.HttpContext.Current.Server.MapPath(uploadFolder);
    //     Directory.CreateDirectory(root);
    //     return new MultipartFormDataStreamProvider(root);
    // }

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