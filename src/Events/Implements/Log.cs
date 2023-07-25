using Hosihikari.Logging;

namespace Hosihikari.Minecraft.Extension.Events.Implements;

public class Log
{
    public static Logger Logger { get; } = new("Events");
}
