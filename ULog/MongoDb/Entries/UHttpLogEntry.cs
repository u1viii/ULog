namespace ULog.MongoDb.Entries;

public class UHttpLogEntry
{
    public string TableName { get; set; }
    public IEnumerable<URequestEntryResult> Results { get; set; }
}
