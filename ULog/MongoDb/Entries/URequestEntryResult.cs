using MongoDB.Bson;

namespace ULog.MongoDb.Entries;

public class URequestEntryResult
{
    public BsonDocument? Data { get; set; }
    public string? User { get; set; }
    public string? EndPoint { get; set; }
    public UResponseEntryResult? Response { get; set; } = null;
    public string? DateTime { get; set; }
}
