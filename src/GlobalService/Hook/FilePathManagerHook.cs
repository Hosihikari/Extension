using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class FilePathManagerHook()
    : HookBase<FilePathManagerHook.HookDelegate>(Core.FilePathManager.Original.Constructor_FilePathManager)
{
    internal delegate Pointer<Core.FilePathManager> HookDelegate(Pointer<Core.FilePathManager> @this, Reference<Core.Path> path, bool a3);

    public override HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(FilePathManagerHook));
            Pointer<Core.FilePathManager> result = Original(@this, a2, a3);
            Global.FilePathManager.Instance = @this.Target;
            TryUninstall();
            return result;
        };
}
