using Microsoft.AspNetCore.Mvc;
using RipStainAPI;
using RipStainAPI.Models;
using RipStainAPI.Services;

[ApiController]
[Route("[controller]")]
public class VerifiedReportController : ControllerBase
{
    private readonly VerifiedReportService _verifiedReportService;

    public VerifiedReportController(VerifiedReportService verifiedReportService)
    {
        _verifiedReportService = verifiedReportService;
    }

    [HttpGet]
    public async Task<List<VerifiedReport>> Get() =>
        await _verifiedReportService.GetAsync();

    [HttpPost]
    public async void Post(VerifiedDTO verifiedDTO)
    {
        await _verifiedReportService.VerifyReportAsync(verifiedDTO.report, verifiedDTO.foodItem);

    }

    [HttpGet("/stats/topten/")]
    public async Task<List<TopItemDTO>> TopTenReported() =>
        await _verifiedReportService.TopTenReported();
    
}

    /* [HttpGet("/stats/TimeSpanStats/")]
    public async Task AnnualMonthlyAsync(int year) =>

         _verifiedReportService.ReportsMonthlyYearl();         
    }
     */


