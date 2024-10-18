using RipStainAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MongoDB.Driver.Linq;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Bson;
using System.Security.Cryptography.X509Certificates;

namespace RipStainAPI.Services;
 public delegate string Reverse(string s);
public class VerifiedReportService
{
    private readonly IMongoCollection<VerifiedReport> _verifiedreports;
    private readonly IMongoCollection<FoodItem> _fooditems;


    

    public VerifiedReportService(
        IOptions<RipStainDbSettings> reportDbSettings)
    {
        var mongoClient = new MongoClient(
            reportDbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            reportDbSettings.Value.DatabaseName);

        _verifiedreports = mongoDatabase.GetCollection<VerifiedReport>(
            reportDbSettings.Value.VerifiedReportsCollection);

        _fooditems = mongoDatabase.GetCollection<FoodItem>(
            reportDbSettings.Value.FoodItemsCollection);

        
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
            FoodItemId = foodItem.Id,
            FoodItemName = foodItem.ItemName,
            Quantity = report.Quantity,
            Value = (double)(report.Quantity * foodItem.ItemPrice),
            ReportedTime = report.ReportedTime
        };
        await _verifiedreports.InsertOneAsync(verifiedReport);
        return verifiedReport;

    }

    #region Analysis methodds

    public Task<List<ReportItemDTO>> TimeSpanReport(string date)
    {

        var queryableCollection = _verifiedreports.AsQueryable();
        var reports = queryableCollection
            .GroupBy(item => new{
                item.FoodItemName,
                item.Value,
                item.FoodItemId
            })
            .Select(report => new ReportItemDTO
            {
                FoodItemId = report.Key.FoodItemId,
                ItemName = report.Key.FoodItemName,
                SumValue = report.Sum(report => report.Value)
            });

            /*.
            .Select(g => new GroupedResult
            {
                Category = g.Key.Category,
                TotalAmount = g.Sum(doc => doc.Amount),
                OtherField1 = g.Key.OtherField1,
                OtherField2 = g.Key.OtherField2
            })
            .ToListAsync();*/


        return reports.ToListAsync();
    }

    public Task<List<ReportItemDTO>> TopTenReported() {
        var queryableCollection = _verifiedreports.AsQueryable();


        var groupedItems = queryableCollection
            .GroupBy(item => item.FoodItemId)
            .Select(group => new ReportItemDTO
            {
                FoodItemId = group.Key,
                ItemName = "item => item.FoodItemName",
                SumValue = group.Sum(item => item.Value)
            });

          
        return groupedItems.ToListAsync();


        }
    public Task<ReportItemDTO> ReportByDepartment(){

        var queryableCollection = _verifiedreports.AsQueryable();
        var groupedItems = queryableCollection
            .GroupBy(item => item.Department)
            .Select(group => new ReportItemDTO
            {
                FoodItemId = group.Key,
                ItemName = (item => item.Department),
                SumValue = group.Sum(item => item.Value)
                
            });

          
        return groupedItems.ToListAsync();
        
    }

   

   
    #endregion
}
       



