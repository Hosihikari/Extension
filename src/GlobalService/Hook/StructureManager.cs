using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class StructureManagerHook()
    : HookBase<StructureManagerHook.HookDelegateType>(StructureManager.Original.Constructor_StructureManager)
{
    protected override HookDelegateType HookDelegate =>
        (@this, rpManager) =>
        {
            Original(@this, rpManager);
            Global.StructureManager.Instance = @this.Target;
            TryUninstall();
        };

    internal delegate void HookDelegateType(Pointer<StructureManager> @this, Reference<ResourcePackManager> rpManager);
}