using RipStainAPI.Models;
using RipStainAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace RipStainAPI.Controllers;


[Route("[controller]")]
[ApiController]
public class UploadController: ControllerBase {
    public readonly UploadService _uploadService;

    public UploadController (UploadService uploadService){
        _uploadService = uploadService;

    }
    // POST: api/files/upload
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected.");
        }

        try
        {
            using (var stream = file.OpenReadStream())
            {
                var fileId = await _uploadService.UploadFileAsync(stream, file.FileName, file.ContentType);
                return Ok(new { FileId = fileId.ToString() });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
        }
    }

    // GET: api/files/download/{id}
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId fileId))
        {
            return BadRequest("Invalid file ID.");
        }

        try
        {
            var stream = await _uploadService.DownloadFileAsync(fileId);
            var fileInfo = await _uploadService.GetFileInfoAsync(fileId);

            return File(stream, fileInfo.Metadata.GetValue("contentType").AsString, fileInfo.Filename);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error downloading file: {ex.Message}");
        }
    }

    // GET: api/files/metadata/{id}
    [HttpGet("metadata/{id}")]
    public async Task<IActionResult> GetFileMetadata(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId fileId))
        {
            return BadRequest("Invalid file ID.");
        }

        try
        {
            var fileInfo = await _uploadService.GetFileInfoAsync(fileId);
            return Ok(new
            {
                FileId = fileInfo.Id.ToString(),
                FileName = fileInfo.Filename,
                ContentType = fileInfo.Metadata.GetValue("contentType").AsString,
                Length = fileInfo.Length,
                UploadDate = fileInfo.UploadDateTime
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error fetching file metadata: {ex.Message}");
        }
    }
}
