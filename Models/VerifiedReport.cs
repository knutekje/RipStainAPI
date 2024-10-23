using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

public class VerifiedReport () {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("foodItemId")]
    public required string FoodItemId { get; set; }
    
    [BsonElement("foodItemName")]
    public required string FoodItemName { get; set; }

    [BsonElement("quantity")]
    public double Quantity { get; set; }

    [BsonElement("value")]
    public double Value { get; set; }

    [BsonElement("deparment")]
    public required string Department { get; set; }

    [BsonElement("reportedTime")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset ReportedTime { get; set; }


}