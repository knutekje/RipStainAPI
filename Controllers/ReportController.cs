using RipStainAPI.Models;
using RipStainAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace RipStainAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService){
        _reportService = reportService;
    }
    
    

    [HttpGet]
    public async Task<List<Report>> Get() =>
        await _reportService.GetAsync();

        
    [HttpPost]
    public async Task<IActionResult> Post(Report report){
        await _reportService.CreateAsync(report);
        return CreatedAtAction(nameof(Get), new {id = report.Id}, report);
    }
    

}