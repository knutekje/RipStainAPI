using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

public class VerifiedReport () {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("foodItem")]
    public required FoodItem FoodItem { get; set; }

    [BsonElement("quantity")]
    public double Quantity { get; set; }

    [BsonElement("value")]
    public double Value { get; set; }

    [BsonElement("reportedTime")]
    public DateTimeOffset ReportedTime { get; set; }


}