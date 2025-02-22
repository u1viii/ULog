using MongoDB.Bson;
using ULog.MongoDb.Entries;

namespace ULog.Interfaces;

interface IHttpULogger
{
    Task LogRequestAsync(URequestEntry data);
    Task LogResponseAsync(UResponseEntry data, ObjectId id);
}
