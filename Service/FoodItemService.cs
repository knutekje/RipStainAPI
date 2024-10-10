using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RipStainAPI.Models;

public class FoodItemService{
    private readonly IMongoCollection<FoodItem> _fooditems;

    public FoodItemService (
        IOptions<RipStainDbSettings> fooditemDbSettings){
        var mongoClient = new MongoClient(
            fooditemDbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            fooditemDbSettings.Value.DatabaseName);
            
        _fooditems = mongoDatabase.GetCollection<FoodItem>(
            fooditemDbSettings.Value.FoodItemsCollection);

    }

    public async Task<List<FoodItem>> GetAsync() =>
        await _fooditems.Find(_ => true).ToListAsync();
    
}