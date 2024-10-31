using RipStainAPI.Models;
using RipStainAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace RipStainAPI.Controllers;


//[Authorize]
[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase {
    private readonly ReportService _reportService;
    private readonly UploadService _uploadService;

    public ReportController(ReportService reportService, UploadService uploadService) {
        _reportService = reportService;
        _uploadService = uploadService;
    }



    [HttpGet]
    public async Task<List<Report>> Get() =>
        await _reportService.GetAsync();


    [HttpPost]
    public async Task<IActionResult> Post([FromForm]ReportDTO reportDTO)
    {
		
		Report report = JsonSerializer.Deserialize<Report>(reportDTO.JsonObject);
      
        report.ReportedTime = DateTimeOffset.Now;
        IFormFile file = reportDTO.File;
        report.FileId = await UploadPost(file);
        await _reportService.CreateAsync(report);
        return CreatedAtAction(nameof(Get), new { id = report.Id }, report);
    }

    private async Task<string> UploadPost(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ("File not selected.");
        }

        try
        {
            using (var stream = file.OpenReadStream())
            {
                var fileId = await _uploadService.UploadFileAsync(stream, file.FileName, file.ContentType);
                return fileId.ToString();
            }
        }
        catch (Exception ex)
        {
            return  ($"Error uploading file: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<Report> GetOne(string id) =>
        await _reportService.GetAsync(id);
    

    [HttpDelete("{id}")]
    public async void Delete(string id) {
        await _reportService.RemoveAsync(id);
    }
    
    
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
}

    

