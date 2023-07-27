using Hosihikari.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public static class Log
{
    public static Logger Logger { get; } = new("GlobalService");
}
