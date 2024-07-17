
# ULog Integration

ULog is a library that uses MongoDb to store both HTTP and manual logs in your database. It's straightforward to use and integrate into your .NET project.

## Installation

To integrate ULog into your .NET project, add the following line:

```csharp
builder.Services.AddULogger(connectionString, options, queueCount);
```

## Configuration

The `options` object is of type `ULogOptions` and takes the following default values:

```csharp
public class ULogOptions
{
    public string? Authorize { get; set; } = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string ManualLogDbName { get; set; } = "logs";
    public string HttpLogDbName { get; set; } = "httplogs";
    public string ManualCollectionName { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
    public string HttpCollectionName { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
}
```

The `queueCount` parameter determines how many logs are queued, with a default value of `2000`.

It's recommended to specify the `Authorize` section yourself when integrating with IoT.

## UI Usage

To use the ULog UI, add the following line to your setup:

```csharp
app.UseULoggerUI();
```

Navigate to `localhost/ULog/index.html` to view your logs in a table format.

## Example

Here's a simple example to get you started:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddULogger("your-connection-string", new ULogOptions
{
    Authorize = "your-authorize-value",
    ManualLogDbName = "your-logs-db",
    HttpLogDbName = "your-httplogs-db",
    ManualCollectionName = "your-manual-collection",
    HttpCollectionName = "your-http-collection"
}, 2000);

var app = builder.Build();

app.UseULoggerUI();

app.Run();
```

## Conclusion

ULog provides a simple and efficient way to manage logs in your .NET application using MongoDb. By following the above steps, you can quickly set up and start using ULog in your projects.

---

Thank you for using ULog! If you encounter any issues or have questions, feel free to reach out.
