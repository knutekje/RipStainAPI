

using MongoDB.Driver.GridFS;

namespace RipStainAPI.Models;

public class RipStainDbSettings {
    public string ConnectionString {get; set; } = null!;

    public string DatabaseName {get; set; } = null!;

    public string ReportsCollection {get; set; } = null!;

    public string FoodItemsCollection {get; set; } = null!;
    public string VerifiedReportsCollection { get; set; } = null!;
    
    public GridFSBucket ImagesBucket { get; set; }

   
}
