using RipStainAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MongoDB.Driver.Linq;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Bson;

namespace RipStainAPI.Services;

public class VerifiedReportService
{
    private readonly IMongoCollection<VerifiedReport> _verifiedreports;

    public VerifiedReportService(
        IOptions<RipStainDbSettings> reportDbSettings)
    {
        var mongoClient = new MongoClient(
            reportDbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            reportDbSettings.Value.DatabaseName);

        _verifiedreports = mongoDatabase.GetCollection<VerifiedReport>(
            reportDbSettings.Value.VerifiedReportsCollection);
    }
    public async Task<List<VerifiedReport>> GetAsync() =>
            await _verifiedreports.Find(_ => true).ToListAsync();

    public async Task CreateAsync(VerifiedReport verifiedReport)
    {

        await _verifiedreports.InsertOneAsync(verifiedReport);


    }

    public async Task<VerifiedReport> VerifyReportAsync( Report report, FoodItem foodItem)
    {
        VerifiedReport verifiedReport = new()
        {
            FoodItem = foodItem,
            Quantity = report.Quantity,
            Value = (double)(report.Quantity * foodItem.ItemPrice),
            ReportedTime = report.ReportedTime
        };
        await _verifiedreports.InsertOneAsync(verifiedReport);
        return verifiedReport;

    }

    #region Analysis methodds

    /*
    Monthy and yearly
    */

    public void ReportsMonthlyYearl(DateTimeOffset date){
        var queryableCollection = _verifiedreports.AsQueryable();
        var query = queryableCollection
        .Where(report => report.ReportedTime.ToString("yyyy") == "2024");

        foreach(var item in query){
            System.Console.WriteLine(item);
        }
        

    }

    
 


    #endregion
}
       



