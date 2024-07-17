using MongoDB.Bson;
using ULog.Enums;
using ULog.MongoDb.Entries;

namespace ULog.Implements;

public interface IULogger
{
    string AuthorizeUser();
    Task<IEnumerable<ULogEntry>> GetLogAsync();
    Task<IEnumerable<URequestEntryResult>> GetHttpLogAsync(UHttpLogQuery query);
    Task<IEnumerable<string>> GetHttpCollectionsAsync();
    Task LogAsync(ULogLevel level, string message, string? userName = null, ActionType actionType = ActionType.None, Dictionary<string, object>? additionalInfo = null);
    Task LogRequestAsync(URequestEntry data);
    Task LogResponseAsync(UResponseEntry data, ObjectId id);
}
