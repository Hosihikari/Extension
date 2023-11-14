using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class AllowListFileHook : HookBase<AllowListFileHook.HookDelegate>
{
    internal unsafe delegate Pointer<AllowListFile> HookDelegate(Pointer<AllowListFile> @this, void* a2);

    public AllowListFileHook()
        : base(AllowListFile.Original.Constructor_AllowListFile) { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2) =>
        {
            Log.Logger.Trace(nameof(AllowListFileHook));
            var result = Original(@this, a2);
            Global.AllowListFile.Instance = new AllowListFile(@this);
            TryUninstall();
            return result;
        };
}