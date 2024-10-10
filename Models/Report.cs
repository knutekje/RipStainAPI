using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RipStainAPI;

public class Report{
    //[BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
    public ObjectId Id { get; set; }
    
    [BsonElement("name")]
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
    public decimal Quantity {get; set;}
    public DateTimeOffset ReportedTime { get; set; }
}

   
