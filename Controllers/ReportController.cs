using RipStainAPI.Models;
using RipStainAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace RipStainAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase {
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService) {
        _reportService = reportService;
    }



    [HttpGet]
    public async Task<List<Report>> Get() =>
        await _reportService.GetAsync();


    [HttpPost]
    public async Task<IActionResult> Post(Report report) {
        report.ReportedTime = DateTime.Now;
        await _reportService.CreateAsync(report);
        return CreatedAtAction(nameof(Get), new { id = report.Id }, report);
    }

    [HttpGet("{id}")]
    public async Task<Report> GetOne(string id) =>
        await _reportService.GetAsync(id);
    

    [HttpDelete("{id}")]
    public async void Delete(string id) {
        await _reportService.RemoveAsync(id);
    }
}

    

