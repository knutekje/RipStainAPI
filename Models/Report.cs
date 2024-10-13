using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RipStainAPI;

public class Report{
    //[BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
    public string Id { get; set; }
    
    [BsonElement("itemName")]
    public required string ItemName { get; set; }
    [BsonElement("description")]
    public string? Description { get; set; }
    [BsonElement("quantity")]
    public double Quantity {get; set;}
    [BsonElement("reportedTime")]
    public DateTimeOffset ReportedTime { get; set; }
}

   
