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
    public async Task<List<ReportItemDTO>> TopTenReported(int year = 2024, int month = 1) =>
        await _verifiedReportService.TopTenReported(year, month);

    [HttpGet("/stats/department/")]
    public async Task<List<ReportItemDTO>> ByDepartment(int year = 2024, int month = 1) =>
        await _verifiedReportService.ReportByDepartment(year, month);

}
    
    


