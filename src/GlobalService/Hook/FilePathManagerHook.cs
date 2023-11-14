using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class FilePathManagerHook : HookBase<FilePathManagerHook.HookDelegate>
{
    internal unsafe delegate Pointer<Core.FilePathManager> HookDelegate(Pointer<Core.FilePathManager> @this, Reference<Core.Path> path, bool a3);

    public FilePathManagerHook()
        : base(Core.FilePathManager.Original.Constructor_FilePathManager) { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            Log.Logger.Trace(nameof(FilePathManagerHook));
            var result = Original(@this, a2, a3);
            Global.FilePathManager.Instance = @this.Target;
            TryUninstall();
            return result;
        };
}
