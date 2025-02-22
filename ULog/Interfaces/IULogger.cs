using MongoDB.Bson;
using ULog.Enums;
using ULog.MongoDb.Entries;

namespace ULog.Interfaces;

public interface IULogger
{
    Task<IEnumerable<ULogEntry>> GetLogAsync();
    Task<(IEnumerable<URequestEntryResult> table, int count)> GetHttpLogAsync(UHttpLogQuery query);
    Task<IEnumerable<string>> GetHttpCollectionsAsync();
    Task LogAsync(ULogLevel level, string message, string? userName = null, ActionType actionType = ActionType.None, Dictionary<string, object>? additionalInfo = null);
}