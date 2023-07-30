using Hosihikari.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public static class Log
{
    private static readonly Lazy<Logger> _logger = new(() =>
    {
        var instance = new Logger(nameof(GlobalService));
        instance.SetupConsole();
        return instance;
    });
    public static Logger Logger => _logger.Value;
}
