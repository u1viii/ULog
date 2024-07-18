using System.Security.Claims;

namespace ULog.MongoDb.Entries;

public class ULogOptions
{
    public string[] Claims { get; set; } = [ClaimTypes.Name];
    internal string Authorize { get; set; }
    public string ManualLogDbName { get; set; } = "logs";
    public string HttpLogDbName { get; set; } = "httplogs";
    public string ManualCollectionName { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
    public string HttpCollectionName { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
}