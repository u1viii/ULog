namespace ULog.MongoDb.Entries;
public class UResponseEntryResult
{
    public string? DateTime { get; set; }
    public double SecondDiff { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
