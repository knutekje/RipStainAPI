using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

public class Picture{

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
    public long Id { get; set; }

    [BsonElement("itemName")]
    public required string FileName { get; set;}


    [BsonElement("itemName")]
    public required string FilePath { get; set;}


    [BsonElement("itemName")]
    public required string Description { get; set;}

    
    

    
}