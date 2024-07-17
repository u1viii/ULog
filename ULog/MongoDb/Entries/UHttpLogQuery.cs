namespace ULog.MongoDb.Entries;

public class UHttpLogQuery
{
    public string Q { get; set; } = null;
    public string Name { get; set; } = null;
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 25;
}
