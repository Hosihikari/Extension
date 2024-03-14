using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Shared.Hook;

internal sealed class FilePathManagerHook()
    : HookBase<FilePathManagerHook.HookDelegateType>(Core.FilePathManager.Original.Constructor_FilePathManager)
{
    protected override HookDelegateType HookDelegate =>
        (@this, a2, a3) =>
        {
            Pointer<Core.FilePathManager> result = Original(@this, a2, a3);
            Global.FilePathManager.Instance = @this.Target;
            TryUninstall();
            return result;
        };

    internal delegate Pointer<Core.FilePathManager> HookDelegateType(Pointer<Core.FilePathManager> @this,
        Reference<Core.Path> path, bool a3);
}