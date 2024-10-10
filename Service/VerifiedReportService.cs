using RipStainAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RipStainAPI.Services;

public class VerifiedReportService {
    private readonly IMongoCollection<VerifiedReport> _verifiedreports;

    public VerifiedReportService(
        IOptions<RipStainDbSettings> reportDbSettings){
            var mongoClient = new MongoClient(
                reportDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                reportDbSettings.Value.DatabaseName);
            
            _verifiedreports = mongoDatabase.GetCollection<VerifiedReport>(
                reportDbSettings.Value.VerifiedReportsCollection);
        } 
        public async Task<List<VerifiedReport>> GetAsync() =>
                await _verifiedreports.Find(_ => true).ToListAsync();

        public async Task CreateAsync(VerifiedReport verifiedReport) {
            

            await _verifiedreports.InsertOneAsync(verifiedReport);


        }

        public async Task<VerifiedReport> VerifyReportAsync(FoodItem foodItem, Report report){
        VerifiedReport verifiedReport = new(){
            FoodItem = foodItem,
            Quantity = report.Quantity,
            Value = (double)(report.Quantity * foodItem.ItemPrice),
            ReportedTime = report.ReportedTime
        };
            await _verifiedreports.InsertOneAsync(verifiedReport);
        return verifiedReport;

        }

}