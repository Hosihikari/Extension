using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class StructureManagerHook : HookBase<StructureManagerHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(Pointer<StructureManager> @this, Reference<ResourcePackManager> rpManager);

    public StructureManagerHook()
        : base(StructureManager.Original.Constructor_StructureManager)
    { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, rpManager) =>
        {
            Log.Logger.Trace(nameof(StructureManagerHook));
            Original(@this, rpManager);
            Global.StructureManager.Instance = @this.Target;
            TryUninstall();
        };
}