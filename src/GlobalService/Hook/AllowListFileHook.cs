using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class AllowListFileHook : HookBase<AllowListFileHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(void* @this, void* a2);

    public AllowListFileHook()
        : base("_ZN13AllowListFileC2ERKN4Core4PathE") { }

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