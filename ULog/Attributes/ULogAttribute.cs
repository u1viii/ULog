using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System.Text.Json;
using ULog.Attributes;
using ULog.Implements;
using ULog.MongoDb.Entries;


public class ULogAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasDisableULogAttribute = context.ActionDescriptor.EndpointMetadata
            .Any(em => em is DisableULogAttribute);

        if (hasDisableULogAttribute)
        {
            await next();
            return;
        }
        var _logger = context.HttpContext.RequestServices.GetRequiredService<IULogger>();
        var _loggerOptions = context.HttpContext.RequestServices.GetRequiredService<ULogOptions>();
        
        var claimValues = context.HttpContext.User.Claims
            .Where(c => _loggerOptions.Claims.Contains(c.Type))
            .Select(x => x.Value.Trim());
        if (claimValues.Any())
        {
            _loggerOptions.Authorize = string.Join(" ", claimValues);
        }
        else
        {
            _loggerOptions.Authorize = context.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        BsonDocument requestBody = new();

        context.HttpContext.Request.EnableBuffering();

        using (var streamReader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true))
        {
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin); // Ensure you're at the start of the stream
            var bodyString = await streamReader.ReadToEndAsync(); // Read the body as a string

            if (!string.IsNullOrEmpty(bodyString))
            {
                try
                {
                    var jsonDocument = JsonDocument.Parse(bodyString); // Use JsonDocument to parse JSON
                    requestBody = BsonDocument.Parse(jsonDocument.RootElement.ToString()); // Convert to BsonDocument if needed
                }
                catch (Exception ex)
                {
                    // Handle error and preserve the raw body string if JSON parsing fails
                    requestBody.Add("Body", bodyString);
                }
            }

            // Reset the stream position for further use
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
        }


        var queryParams = context.HttpContext.Request.Query
            .ToDictionary(q => q.Key, q => (object)q.Value.ToString());

        foreach (var param in queryParams)
        {
            requestBody[param.Key] = BsonValue.Create(param.Value);
        }
        var headers = context.HttpContext.Request.Headers
            .ToDictionary(h => h.Key, h => (object)h.Value.ToString());
        var controllerParams = context.ActionDescriptor.Parameters.Select(p => p.Name);
        var headersBson = new BsonDocument();
        foreach (var header in headers)
        {
            if (controllerParams.Contains(header.Key, System.StringComparer.OrdinalIgnoreCase))
            {
                headersBson[header.Key] = BsonValue.Create(header.Value);
            }
        }
        if (headersBson.Any())
        {
            requestBody.Add("Headers", headersBson);
        }

        var id = ObjectId.GenerateNewId();
        var requestTime = DateTime.Now;

        await _logger.LogRequestAsync(new URequestEntry
        {
            _id = id,
            Data = requestBody,
            User = _loggerOptions.Authorize,
            EndPoint = context.HttpContext.Request.Path,
            DateTime = requestTime,
        });

        var resultContext = await next();

        await _logger.LogResponseAsync(new UResponseEntry
        {
            StatusCode = context.HttpContext.Response.StatusCode,
            Message = resultContext.Exception == null ? "Action completed successfully" : $"Action failed with exception: {resultContext.Exception.Message}",
            SecondDiff = (DateTime.Now - requestTime).TotalSeconds,
            DateTime = DateTime.Now
        }, id);
    }
}
