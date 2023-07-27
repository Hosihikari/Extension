using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class FilePathManagerHook : HookBase<FilePathManagerHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(void* @this, void* a2, void* a3);

    public FilePathManagerHook()
        : base("_ZN4Core15FilePathManagerC2ERKNS_4PathEb") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            Log.Logger.Trace(nameof(FilePathManagerHook));
            var result = Original(@this, a2, a3);
            Global.FilePathManager.Instance = new Core.FilePathManager(@this);
            TryUninstall();
            return result;
        };
}
