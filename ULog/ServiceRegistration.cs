using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using ULog.Implements;
using ULog.MongoDb;
using ULog.MongoDb.Entries;


namespace ULog;

public static class ServiceRegistration
{
    public static IServiceCollection AddULogger(this IServiceCollection services, string connectionString, ULogOptions? options = null, int queueCount = 2000)
    {
        ULogOptions _options = options ?? new ULogOptions();
        services.AddSingleton<IBTQ>(_ =>
        {
            return new BTQ(queueCount);
        });
        services.AddHostedService<QHS>();
        services.AddSingleton<IULogger>(provider =>
        {
            if (_options.Authorize == null)
            {
                if (!services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(IHttpContextAccessor)))
                {
                    services.AddHttpContextAccessor();
                }
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var claimValues = httpContextAccessor.HttpContext.User.Claims.Where(c => _options.Claims.Contains(c.Type)).Select(x => x.Value.Trim());
                _options.Authorize = string.Join(" ", claimValues);
            }
            var backgroundTaskQueue = provider.GetRequiredService<IBTQ>();
            return new MongoDBLogger(connectionString, _options, backgroundTaskQueue);
        });
        return services;
    }
    public static IApplicationBuilder UseULoggerUI(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ULogUIMiddleware>();
    }
}
