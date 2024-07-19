using Microsoft.AspNetCore.Builder;
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
        services.AddSingleton<IBTQ>(_ => new BTQ(queueCount));
        services.AddHostedService<QHS>();
        services.AddSingleton<IULogger>(provider =>
        {
            var backgroundTaskQueue = provider.GetRequiredService<IBTQ>();
            return new MongoDBLogger(connectionString, _options, backgroundTaskQueue);
        });
        services.AddSingleton(_options);
        return services;
    }
    public static IApplicationBuilder UseULoggerUI(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ULogUIMiddleware>();
    }
}
