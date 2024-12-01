using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ULog.MongoDb.Entries;
public class URequestEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public BsonDocument? Data { get; set; }
    public string? User { get; set; }
    public string? EndPoint { get; set; }
    public UResponseEntry? Response { get; set; } = null;
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime DateTime { get; set; } = DateTime.Now;
}
