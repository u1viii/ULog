using Microsoft.AspNetCore.Http;
using ULog.MongoDb.Entries;

namespace ULog.Middlewares
{
    public class HttpLoggingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            string requestBody = string.Empty;
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var requestLog = new URequestBody
            {
                Method = context.Request.Method,
                Body = requestBody,
            };

            context.Items["RequestBody"] = requestLog;

            await _next(context);
        }
    }
}
