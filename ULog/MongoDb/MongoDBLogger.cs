using MongoDB.Bson;
using MongoDB.Driver;
using ULog.Enums;
using ULog.Implements;
using ULog.MongoDb.Entries;

namespace ULog.MongoDb;

public class MongoDBLogger : IULogger
{
    readonly IMongoCollection<ULogEntry> _logCollection;
    readonly IMongoCollection<URequestEntry> _httpCollection;
    readonly IBackgroundTaskQueue _bgService;
    public MongoDBLogger(string connectionString, string databaseName, string collectionName, IBackgroundTaskQueue bgService)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _logCollection = database.GetCollection<ULogEntry>(collectionName);
        _httpCollection = database.GetCollection<URequestEntry>(collectionName + "logs");
        _bgService = bgService;
    }

    public async Task<IEnumerable<ULogEntry>> GetLogAsync()
    {
        var group = new BsonDocument
        {
            { "$group", new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            { "$dateToString", new BsonDocument
                                {
                                    { "format", "%Y-%m-%d" },
                                    { "date", "$DateTime" }
                                }
                            }
                        }
                    },
                    { "count", new BsonDocument
                        {
                            { "$sum", 1 }
                        }
                    }
                }
            }
        };
        var results = await _logCollection.Aggregate()
        .AppendStage<BsonDocument>(group)
        .ToListAsync();
        return await _logCollection.Find(Builders<ULogEntry>.Filter.Empty).ToListAsync();
    }

    public async Task LogAsync(ULogLevel level, string message, string userName = null, ActionType actionType = ActionType.None, Dictionary<string, object>? additionalInfo = null)
    {
        BsonDocument document = new BsonDocument();
        document.AddRange(additionalInfo ?? new Dictionary<string, object>());
        await _logCollection.InsertOneAsync(new ULogEntry
        {
            ActionType = actionType.ToString(),
            LogLevel = level.ToString(),
            Message = message,
            Username = userName,
            AdditionalInfo = document
        });
    }

    public async Task LogRequestAsync(URequestEntry data)
    {
        await _bgService.QueueBackgroundWorkItemAsync(async token =>
        {
            await _httpCollection.InsertOneAsync(data);
        });
    }

    public async Task LogResponseAsync(UResponseEntry? data, ObjectId id)
    {
        await _bgService.QueueBackgroundWorkItemAsync(async token =>
        {
            var filter = Builders<URequestEntry>.Filter.Eq("_id", id);
            var update = Builders<URequestEntry>.Update.Set(req => req.Response, data);

            await _httpCollection.UpdateOneAsync(filter, update);
        });
    }
}
