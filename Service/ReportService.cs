using RipStainAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RipStainAPI.Services;

public class ReportService {

    private readonly IMongoCollection<Report> _reports;

    public ReportService(
        IOptions<RipStainDbSettings> reportDbSettings){
            var mongoClient = new MongoClient(
                reportDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                reportDbSettings.Value.DatabaseName);
            
            _reports = mongoDatabase.GetCollection<Report>(
                reportDbSettings.Value.ReportsCollection);
        }
        public async Task<List<Report>> GetAsync() =>
            await _reports.Find(_ => true).ToListAsync();

        public async Task<Report?> GetAsync(string id) =>
            await _reports.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();


        public async Task CreateAsync(Report report) =>
            await _reports.InsertOneAsync(report);

        public async Task UpdateAsync(string id, Report reportIn) =>
            await _reports.ReplaceOneAsync(x => (x.Id).ToString() == id, reportIn);

        public async Task RemoveAsync(string id) =>
            await _reports.DeleteOneAsync(x => (x.Id).ToString() == id);
    }
    
    
