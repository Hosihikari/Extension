using Hosihikari.Minecraft.Extension.GlobalService.Hook;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public static class Global
{
    public static GlobalInstance<Core.FilePathManager> FilePathManager { get; } = new();

    internal static void Init()
    {
        new FilePathManagerHook().Install();
    }
}
