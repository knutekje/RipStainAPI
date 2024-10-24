using RipStainAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace RipStainAPI.Services;
 public delegate string Reverse(string s);
public class VerifiedReportService
{
    private readonly IMongoCollection<VerifiedReport> _verifiedreports;
/*     private readonly IMongoCollection<FoodItem> _fooditems;
 */

    

    public VerifiedReportService(
        IOptions<RipStainDbSettings> reportDbSettings)
    {
        var mongoClient = new MongoClient(
            reportDbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            reportDbSettings.Value.DatabaseName);

        _verifiedreports = mongoDatabase.GetCollection<VerifiedReport>(
            reportDbSettings.Value.VerifiedReportsCollection);

    /*     _fooditems = mongoDatabase.GetCollection<FoodItem>(
            reportDbSettings.Value.FoodItemsCollection); */

        
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
            Department = report.Department,
            FoodItemName = foodItem.ItemName,
            Quantity = report.Quantity,
            Value = (double)(report.Quantity * foodItem.ItemPrice),
            ReportedTime = report.ReportedTime
        };
        await _verifiedreports.InsertOneAsync(verifiedReport);
        return verifiedReport;

    }

    #region Analysis methodds

     public IMongoQueryable<VerifiedReport> TimeSpanReport(int date, int month){

        var collection = _verifiedreports.AsQueryable();

        // Define your date range using DateTimeOffset
        DateTimeOffset startDate = new DateTimeOffset(date, month, 1, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset endDate = new DateTimeOffset(date, month + 1, 1, 23, 59, 59, TimeSpan.Zero);

        // LINQ query to filter documents within the time frame
        var results = collection
                .AsQueryable()
                .Where(doc => doc.ReportedTime >= startDate && doc.ReportedTime <= endDate);
                

        return (IMongoQueryable<VerifiedReport>)results; 
    } 

    public Task<List<ReportItemDTO>> TopTenReported(int year, int month) {

        var queryableCollection = TimeSpanReport(year, month);

        var reports = queryableCollection
            .GroupBy(item => item.FoodItemName)
            
            .Select(report => new ReportItemDTO
            {
                ItemName = report.Key,
                SumValue = report.Sum(report => report.Value)
            }).OrderByDescending(x => x.SumValue);


          
        return reports.ToListAsync();
        }

    public Task<List<ReportItemDTO>> ReportByDepartment(int year, int month){

        var queryableCollection = TimeSpanReport(year, month);
        var reports = queryableCollection
            .GroupBy(item => new{
                item.Department
                
            })
            .Select(report => new ReportItemDTO
            {
                ItemName = report.Key.Department,
                SumValue = report.Sum(report => report.Value)
            });


          
        return reports.ToListAsync();
        
    }

   

   
    #endregion
}
       



