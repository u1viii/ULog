namespace ULog.MongoDb.Entries;
public class UResponseEntry
{
    public DateTime DateTime;
    public int SecondDiff { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
