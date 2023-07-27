using System.Runtime.CompilerServices;
using Hosihikari.Minecraft.Extension.PackHelper;

namespace Hosihikari.Minecraft.Extension;

internal class EntryPoint
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    internal static void Main() // must be loaded
    {
        LevelTick.InitHook();
        GlobalService.Global.Init();
        PackHelper.Main.Init();
    }
}
