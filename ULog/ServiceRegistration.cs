﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ULog.Implements;
using ULog.MongoDb;
using ULog.MongoDb.Entries;


namespace ULog;

public static class ServiceRegistration
{
    public static IServiceCollection AddULogger(this IServiceCollection services, string connectionString, ULogOptions? options = null, int queueCount = 2000)
    {
        ULogOptions _options = options ?? new ULogOptions();
        return services.AddServices(connectionString, _options, queueCount);
    }
    static IServiceCollection AddServices(this IServiceCollection services, string connectionString, ULogOptions? _options = null, int queueCount = 2000)
    {
        services.AddSingleton<IBTQ>(_ => new BTQ(queueCount));
        services.AddHostedService<QHS>();
        services.AddSingleton<IULogger>(provider =>
        {
            var backgroundTaskQueue = provider.GetRequiredService<IBTQ>();
            return new MongoDBLogger(connectionString, _options, backgroundTaskQueue);
        });
        services.AddSingleton(_options);
        services.AddSingleton(new UConfiguration());
        return services;
    }

    public static IApplicationBuilder UseULoggerUI(this IApplicationBuilder app, UConfiguration? configuration = null)
    {
        var config = app.ApplicationServices.GetRequiredService<UConfiguration>();
        if (configuration is not null)
        {
            config.UseCookies = configuration.UseCookies;
            config.Keyword = configuration.Keyword;
        }
        return app.UseMiddleware<ULogUIMiddleware>();
    }
}
