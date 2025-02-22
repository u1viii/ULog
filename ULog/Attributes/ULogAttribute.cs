using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System.Reflection;
using System.Text.Json;
using ULog.Attributes;
using ULog.Interfaces;
using ULog.MongoDb.Entries;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ULogAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check if the action has DisableULogAttribute
        var hasDisableULogAttribute = context.ActionDescriptor.EndpointMetadata
            .Any(em => em is DisableULogAttribute);

        if (hasDisableULogAttribute)
        {
            await next();
            return;
        }

        var httpContext = context.HttpContext;
        var _logger = httpContext.RequestServices.GetRequiredService<IHttpULogger>();
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
            var actionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            BsonDocument bson = null;
            if (actionDescriptor != null)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var parameter = actionDescriptor.Parameters.FirstOrDefault().ParameterType;
                var data = JsonSerializer.Deserialize(requestBody.Body, parameter, options);
                if (data != null)
                {
                    bson = BsonDocument.Parse(JsonSerializer.Serialize(MaskSensitiveData(data)));
                }
            }
            if (bson is not null)
            {
                request.Add("Body", bson);
            }
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
    /// <summary>
    /// Masking datas which has SensitiveDataAttribute
    /// </summary>
    /// <param name="data">Parameter object</param>
    /// <returns></returns>
    object MaskSensitiveData(object data)
    {
        if (data == null) return data;
        var allProps = data.GetType().GetProperties();
        foreach (var prop in allProps)
        {
            if (prop.IsDefined(typeof(SensitiveDataAttribute), true))
            {
                var attribute = prop.GetCustomAttribute<SensitiveDataAttribute>()!;
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(data, attribute.Mask);
                else
                    prop.SetValue(data, default);
            }
            else if (prop.PropertyType.IsClass)
            {
                var value = prop.GetValue(data);
                if (value != null && !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string))
                {
                    prop.SetValue(data, MaskSensitiveData(value));
                }
            }
        }
        return data;
    }
}
