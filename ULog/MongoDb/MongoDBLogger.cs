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
    readonly IBTQ _bgService;
    readonly MongoClient _client;
    readonly IMongoDatabase _database;
    readonly IMongoDatabase _httpDatabase;
    readonly string? _authorizeUser;
    public MongoDBLogger(string connectionString, ULogOptions options, IBTQ bgService)
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(options.ManualLogDbName);
        _httpDatabase = _client.GetDatabase(options.HttpLogDbName);
        _logCollection = _database.GetCollection<ULogEntry>(options.ManualCollectionName);
        _httpCollection = _httpDatabase.GetCollection<URequestEntry>(options.HttpCollectionName);
        _bgService = bgService;
        _authorizeUser = options.Authorize;
    }

    public async Task<IEnumerable<string>> GetHttpCollectionsAsync()
    {
        var names = await (await _httpDatabase.ListCollectionNamesAsync()).ToListAsync();
        names.Reverse();
        return names;
    }

    public async Task<IEnumerable<UHttpLogEntry>> GetHttpLogAsync()
    {
        var names = await (await _httpDatabase.ListCollectionNamesAsync()).ToListAsync();
        List<UHttpLogEntry> entries = new();
        foreach (var name in names)
        {
            var collection = _httpDatabase.GetCollection<URequestEntry>(name);
            var results = (await collection.Find(Builders<URequestEntry>.Filter.Empty).ToListAsync()).Select(x => new URequestEntryResult
            {
                Data = x.Data ?? new BsonDocument(),
                DateTime = x.DateTime.ToString("HH:mm:ss"),
                EndPoint = x.EndPoint,
                Response = new UResponseEntryResult
                {
                    DateTime = x.Response.DateTime.Value.ToString("HH:mm:ss"),
                    Message = x.Response.Message,
                    SecondDiff = x.Response.SecondDiff,
                    StatusCode = x.Response.StatusCode
                },
                User = x.User
            });
            entries.Add(new UHttpLogEntry
            {
                TableName = name,
                Results = results
            });
        }
        return entries;
    }

    public async Task<(IEnumerable<URequestEntryResult> table, int count)> GetHttpLogAsync(UHttpLogQuery query)
    {
        var names = await (await _httpDatabase.ListCollectionNamesAsync()).ToListAsync();
        List<UHttpLogEntry> entries = new();
        var table = _httpDatabase.GetCollection<URequestEntry>(query.Name);
        if (table == null) throw new Exception();
        var builder = Builders<URequestEntry>.Filter;
        FilterDefinition<URequestEntry> filter = builder.Empty;
        if (!string.IsNullOrWhiteSpace(query.Q))
        {
            var filters = new List<FilterDefinition<URequestEntry>>
                {
                    builder.Regex(entry => entry.User, new BsonRegularExpression(query.Q, "i")),
                    builder.Regex(entry => entry.EndPoint, new BsonRegularExpression(query.Q, "i")),
                    builder.Regex(entry => entry.Response, new BsonRegularExpression(query.Q, "i"))
                };
            filter = builder.Or(filters);
        }
        var a = await table.Find(filter).ToListAsync();
        var result = (await table.Find(filter).Skip((query.Page - 1) * query.Take).Limit(query.Take).ToListAsync()).Select(x => new URequestEntryResult
        {
            Data = x.Data ?? new BsonDocument(),
            DateTime = x.DateTime.ToString("HH:mm:ss.fff"),
            EndPoint = x.EndPoint,
            Response = new UResponseEntryResult
            {
                DateTime = x.DateTime.ToString("HH:mm:ss.fff"),
                Message = x.Response.Message,
                SecondDiff = x.Response.SecondDiff,
                StatusCode = x.Response.StatusCode
            },
            User = x.User
        });
        (IEnumerable<URequestEntryResult>, int) tupleResult = (result, Convert.ToInt32(await table.Find(filter).CountAsync()));
        return tupleResult;
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

    public async Task LogAsync(ULogLevel level, string message, string? userName = null, ActionType actionType = ActionType.None, Dictionary<string, object>? additionalInfo = null)
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

    public string AuthorizeUser() => _authorizeUser;
}
