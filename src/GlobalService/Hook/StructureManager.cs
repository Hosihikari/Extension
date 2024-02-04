using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class StructureManagerHook()
    : HookBase<StructureManagerHook.HookDelegate>(StructureManager.Original.Constructor_StructureManager)
{
    internal delegate void HookDelegate(Pointer<StructureManager> @this, Reference<ResourcePackManager> rpManager);

    public override HookDelegate HookedFunc =>
        (@this, rpManager) =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(StructureManagerHook));
            Original(@this, rpManager);
            Global.StructureManager.Instance = @this.Target;
            TryUninstall();
        };
}