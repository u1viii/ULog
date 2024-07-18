using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ULog.Implements;
using ULog.MongoDb.Entries;


namespace ULog;

public class ULogUIMiddleware
{
    const string FileFolder = "StudioLoggerLibrary.files";
    readonly ULogUIOptions _options;
    readonly StaticFileMiddleware _staticFileMiddleware;
    readonly IULogger _logger;
    public ULogUIMiddleware(RequestDelegate next,
            IHostingEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            IULogger logger
            )
    {
        _options = new ULogUIOptions();
        _logger = logger;
        _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory, _options);
    }
    private StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IHostingEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            ULogUIOptions options)
    {
        var staticFileOptions = new StaticFileOptions
        {
            RequestPath = string.IsNullOrEmpty(options.RoutePrefix) ? string.Empty : $"/{options.RoutePrefix}",
            FileProvider = new EmbeddedFileProvider(typeof(ULogUIMiddleware).GetTypeInfo().Assembly, FileFolder),
        };

        return new StaticFileMiddleware(next, (IWebHostEnvironment)hostingEnv, Options.Create(staticFileOptions), loggerFactory);
    }
    public async Task Invoke(HttpContext httpContext)
    {
        var httpMethod = httpContext.Request.Method;
        var path = httpContext.Request.Path.Value;

        if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase))
        {
            var indexUrl = httpContext.Request.GetEncodedUrl().TrimEnd('/') + "/index.html";
            RespondWithRedirect(httpContext.Response, indexUrl);
            return;
        }

        if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase))
        {
            var obj = httpContext.Request.Query;
            string type = obj["type"];
            if (int.TryParse(type, out int result))
            {
                if (!httpContext.Response.HasStarted)
                {
                    if (result == 0)
                    {
                        await ResponseWithHttpTablesAsync(httpContext.Response);
                    }
                    else if (result == 1)
                    {
                    }
                    else
                    {
                        await _staticFileMiddleware.Invoke(httpContext);
                    }
                    await RespondWithIndexHtml(httpContext.Response);
                }
            }
            else
            {
                await RespondWithTypes(httpContext.Response);
            }
            await _staticFileMiddleware.Invoke(httpContext);
            return;
        }

        await _staticFileMiddleware.Invoke(httpContext);
    }
    private void RespondWithRedirect(HttpResponse response, string location)
    {
        if (!response.HasStarted)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = location;
        }
    }

    private async Task ResponseWithHttpTablesAsync(HttpResponse response)
    {
        if (!response.HasStarted)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";
            var json = new
            {
                attribute = await _logger.GetHttpCollectionsAsync(),
                manual = new string[0]
            };
            var query = response.HttpContext.Request.Query;
            UHttpLogQuery qObj = new UHttpLogQuery();
            if (query.Any(x => x.Key == "tableName"))
            {
                qObj.Name = query.FirstOrDefault(x => x.Key == "tableName").Value;
                if (query.Any(x => x.Key == "q"))
                {
                    qObj.Q = query.FirstOrDefault(x => x.Key == "q").Value;
                }
                if (query.Any(x => x.Key == "page"))
                {
                    qObj.Page = Convert.ToInt32(query.FirstOrDefault(x => x.Key == "page").Value);
                }
                var table = await _logger.GetHttpLogAsync(qObj);
                await WriteHtml(response, json, table);
            }
            else
            {
                await WriteHtml(response, json);
            }
        }
    }
    private async Task RespondWithTypes(HttpResponse response)
    {
        if (!response.HasStarted)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";
            var json = new
            {
                attribute = new string[0],
                manual = new string[0]
            };
            await WriteHtml(response, json);
        }
    }
    private async Task WriteHtml(HttpResponse response, object data, object table = null)
    {
        if (!response.HasStarted)
        {
            using (var stream = _options.IndexHtml())
            {
                var htmlBuilder = new StringBuilder(new StreamReader(stream).ReadToEnd());

                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var jsonTable = table == null ? "[]" : ((IEnumerable<URequestEntryResult>)table).ToJson();


                htmlBuilder.Replace("%(datas)", jsonData);
                htmlBuilder.Replace("%(table)", jsonTable);

                await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            }
        }
    }
    private async Task RespondWithIndexHtml(HttpResponse response)
    {
        if (!response.HasStarted)
        {

            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";
            var json = new
            {
                attribute = new string[0],
                manual = new string[0]
            };
            await WriteHtml(response, json);
        }
    }
}
