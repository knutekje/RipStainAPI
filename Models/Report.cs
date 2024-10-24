using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RipStainAPI;

public class Report{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("itemName")]
    public required string ItemName { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }
    
    [BsonElement("department")]
    public string? Department { get; set; }
    
    [BsonElement("fileId")]
    public string? FileId { get; set; }
    

    [BsonElement("quantity")]
    public double Quantity {get; set;}

    [BsonElement("reportedTime")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset ReportedTime { get; set; }
}

   
