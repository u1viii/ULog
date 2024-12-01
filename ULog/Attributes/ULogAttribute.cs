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

        var httpContext = context.HttpContext;
        var _logger = httpContext.RequestServices.GetRequiredService<IULogger>();
        var _loggerOptions = httpContext.RequestServices.GetRequiredService<ULogOptions>();
        //Coming from middleware
        var requestBody = httpContext.Items["RequestBody"] as URequestBody;

        //Who sent request
        _loggerOptions.Authorize = GetAuthorize(httpContext, _loggerOptions.Claims);

        BsonDocument request = new();

        //Query params
        foreach (var param in GetQueryParams(httpContext))
        {
            request[param.Key] = BsonValue.Create(param.Value);
        }

        //Header params
        var headersBson = GetHeaders(context);

        if (headersBson.Any())
        {
            request.Add("Headers", headersBson);
        }

        //Body params
        if (request is not null && !string.IsNullOrEmpty(requestBody!.Body))
        {
            request.Add("Body", BsonDocument.Parse(requestBody.Body));
        }

        //Start logging
        var id = ObjectId.GenerateNewId();
        var requestTime = DateTime.Now;

        await _logger.LogRequestAsync(new URequestEntry
        {
            _id = id,
            Data = request,
            User = _loggerOptions.Authorize,
            EndPoint = context.HttpContext.Request.Path,
            Method = requestBody?.Method ?? "",
            DateTime = requestTime
        });

        var resultContext = await next();

        await _logger.LogResponseAsync(new UResponseEntry
        {
            StatusCode = context.HttpContext.Response.StatusCode,
            Message = resultContext.Exception == null ? "Working!" : $"Error: {resultContext.Exception.Source}",
            SecondDiff = (DateTime.Now - requestTime).TotalSeconds,
            DateTime = DateTime.Now
        }, id);
    }
    string GetAuthorize(HttpContext context, string[] claims)
    {
        var claimValues = context.User.Claims
            .Where(c => claims.Contains(c.Type))
            .Select(x => x.Value.Trim());
        if (claimValues.Any())
        {
            return string.Join(" ", claimValues);
        }
        else
        {
            return GetIpAddress(context.Request);
        }
    }
    Dictionary<string, object> GetQueryParams(HttpContext context)
    {
        return context.Request.Query
            .ToDictionary(q => q.Key, q => (object)q.Value.ToString());
    }
    BsonDocument GetHeaders(ActionExecutingContext context)
    {
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
        return headersBson;
    }
    string GetIpAddress(HttpRequest request)
    {
        var ipAddress = request?.Headers?["X-Real-IP"].ToString();

        if (!string.IsNullOrEmpty(ipAddress))
            return ipAddress;

        ipAddress = request?.Headers?["X-Forwarded-For"].ToString();

        if (!string.IsNullOrEmpty(ipAddress))
        {
            var parts = ipAddress.Split(',');

            if (parts.Count() > 0)
            {
                ipAddress = parts[0];
            }
            return ipAddress;
        }

        ipAddress = request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        if (!string.IsNullOrEmpty(ipAddress))
            return ipAddress;
        return string.Empty;
    }

}
