using System.Reflection;

namespace ULog;

public class ULogUIOptions
{
    public string RoutePrefix { get; set; } = "ulog";
    public Func<Stream> IndexHtml { get; } = () => typeof(ULogUIOptions)?.GetTypeInfo()?.Assembly?
        .GetManifestResourceStream("StudioLoggerLibrary.index.html");
}
