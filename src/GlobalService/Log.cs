using Hosihikari.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public class Log
{
    public static Logger Logger { get; } = new("GlobalService");
}
