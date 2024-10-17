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

    public async Task<List<VerifiedReport>> ReportsMonthlyYearl(int year)
    {


        DateTimeOffset startDate = new DateTime(year, 1, 1);
        DateTimeOffset endDate = new DateTime(year + 1, 1, 1);

        var filter = Builders<VerifiedReport>.Filter.Gte(x => x.ReportedTime, startDate)
                    & Builders<VerifiedReport>.Filter.Lt(x => x.ReportedTime, endDate);
        var response = await _verifiedreports.Find(filter).ToListAsync();


        return response;
    }







    public Task<List<TopItemDTO>> TopTenReported() {
        var queryableCollection = _verifiedreports.AsQueryable();


        var groupedItems = queryableCollection
            .GroupBy(item => item.FoodItemId)
            .Select(group => new TopItemDTO
            {
                FoodItemId = group.Key,
                ItemName = "MINGERS",
                SumValue = group.Sum(item => item.Value)
            });

          
        return groupedItems.ToListAsync();


        }



    public  string FindName(string key)
        {
        //var saus = _fooditems.Find(x => x.Id == key).First().ItemName;
        return key;
        }
       

    /* Most represented  department
         count OCCURSENCE per departments
     */ 
    #endregion
}
       



