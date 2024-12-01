using MongoDB.Bson.Serialization.Attributes;

namespace ULog.MongoDb.Entries;
public class UResponseEntry
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateTime { get; set; }
    public double SecondDiff { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
