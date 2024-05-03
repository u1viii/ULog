using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System.Security.Claims;
using System.Text.Json;
using ULog.Implements;
using ULog.MongoDb.Entries;

public class ULogAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var _logger = context.HttpContext.RequestServices.GetRequiredService<IULogger>();
        var actionArguments = context.ActionArguments;

        BsonDocument requestBody = new();
        if (context.HttpContext.Request.Method == "POST")
        {
            context.HttpContext.Request.EnableBuffering();
            using var streamReader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
            requestBody = BsonDocument.Parse(JsonSerializer.Serialize(await streamReader.ReadToEndAsync()));
            context.HttpContext.Request.Body.Position = 0;
        }
        else
        {
            var queryParams = context.HttpContext.Request.Query
            .ToDictionary(q => q.Key, q => q.Value.ToString());

            requestBody = BsonDocument.Parse(JsonSerializer.Serialize(queryParams));
        }
        var id = ObjectId.GenerateNewId();
        var requestTime = DateTime.Now;
        await _logger.LogRequestAsync(new URequestEntry
        {
            _id = id,
            Data = requestBody,
            User = context.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value,
            EndPoint = context.HttpContext.Request.Path,
            DateTime = requestTime,
        });
        var resultContext = await next();

        await _logger.LogResponseAsync(new UResponseEntry
        {
            StatusCode = context.HttpContext.Response.StatusCode,
            Message = resultContext.Exception == null ? "Action completed successfully" : $"Action failed with exception: {resultContext.Exception.Message}",
            SecondDiff = DateTime.Now.Second - requestTime.Second,
            DateTime = DateTime.Now
        }, id);

    }
}
