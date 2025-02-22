using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ULog.Enums;

namespace ULog.MongoDb.Entries;
public class ULogEntry
{
    [BsonId]
    public string _id = Guid.NewGuid().ToString();
    public string? LogLevel { get; set; } = nameof(ULogLevel.Info);
    public string? Message { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;
    public string? ActionType { get; set; } = nameof(Enums.ActionType.None);
    public BsonDocument? AdditionalInfo { get; set; } = null;
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime DateTime = DateTime.Now;
}
