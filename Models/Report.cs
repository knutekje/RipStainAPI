using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RipStainAPI;

public class Report{
[BsonId]
[BsonRepresentation(BsonType.ObjectId)]
public ObjectId _id { get; set; }
 
 [BsonElement("name")]
 public required string Title { get; set; }
 public string? Description { get; set; }
 public bool Status { get; set; }
 public decimal Quantity {get; set;}
 public string? ReportedTime { get; set; }
}

   
