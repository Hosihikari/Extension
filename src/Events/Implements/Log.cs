namespace Hosihikari.Minecraft.Extension.Events.Implements;

public class Log
{
    private static readonly Lazy<Logger> _logger = new(() =>
    {
        var instance = new Logger(nameof(Events));
        instance.SetupConsole();
        return instance;
    });
    public static Logger Logger => _logger.Value;
}
