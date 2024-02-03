using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public static class Log
{
    private static readonly Lazy<ILogger> s_logger = new(() =>
    {
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        return factory.CreateLogger(nameof(GlobalService));
    });
    public static ILogger Logger => s_logger.Value;
}
