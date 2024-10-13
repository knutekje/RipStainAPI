using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

public class FoodItem {
    /*
    _id
    itemnr
    itemName
    itemPrice
    itemUnit
    */
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
    public string Id { get; set; }

    [BsonElement("itemnr")] 
    public required int Itemnr { get; set; }

    [BsonElement("itemName")] 
    public string? ItemName { get; set; }

    [BsonElement("itemPrice")] 
    public double? ItemPrice { get; set; }

    [BsonElement("itemUnit")] 
    public string? ItemUnit { get; set; }


}