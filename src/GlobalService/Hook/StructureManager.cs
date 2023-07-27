using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class StructureManagerHook : HookBase<StructureManagerHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(void* @this, void* rpManager);

    public StructureManagerHook()
        : base(
            "_ZN16StructureManagerC2ER19ResourcePackManager") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, rpManager) =>
        {
            Log.Logger.Trace(nameof(StructureManagerHook));
            Original(@this, rpManager);
            Global.StructureManager.Instance = new StructureManager(@this);
            TryUninstall();
        };
}