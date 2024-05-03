using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ULog.Implements;
using ULog.MongoDb;


namespace ULog;

public static class ServiceRegistration
{
    public static IServiceCollection AddULogger(this IServiceCollection services, string connectionString, string db, string table)
    {
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new BackgroundTaskQueue(2000);
        });
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IULogger>(provider =>
        {
            var backgroundTaskQueue = provider.GetRequiredService<IBackgroundTaskQueue>();
            return new MongoDBLogger(connectionString, db, table, backgroundTaskQueue);
        });
        return services;


    }
    public static IApplicationBuilder UseULoggerUI(this IApplicationBuilder app)
    {
        app.Use(async (context, _next) =>
        {
            var logService = context.RequestServices.GetRequiredService<IULogger>();
            if (context.Request.Path.StartsWithSegments("/ulog/index.html"))
            {
                var logs = await logService.GetLogAsync();

                //var responseHtml = "<html><body><h1>Log Records</h1><ul>";
                var responseHtml =
                $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
    <link href='https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/7.2.0/mdb.min.css' rel='stylesheet' />
</head>
<body>
    <div class='container pt-2'>
        <div class='row'>
            <div class='col-md-2'>
    ";
                foreach (var log in logs)
                {
                    responseHtml += $@"<div class='list-group list-group-light'>
 <a href='/ulog/index.html?{log.DateTime}' class='list-group-item list-group-item-action px-3 border-1 my-1 list-group-item-secondary' aria-current='true'>{log.DateTime}</a>
    </div>";
                }
                responseHtml += $@"</div>
        </div>
    </div>
    <script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/7.2.0/mdb.umd.min.js'></script>
</body>
</html>";

                await context.Response.WriteAsync(responseHtml);
            }
            else
            {
                await _next();
                //await _next(context);
            }
        });
        return app;
    }
}
