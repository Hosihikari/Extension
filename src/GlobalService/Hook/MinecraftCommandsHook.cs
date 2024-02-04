using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class MinecraftCommandsHook()
    : HookBase<MinecraftCommandsHook.HookDelegate>(MinecraftCommands.Original.InitCoreEnums)
{
    internal delegate void HookDelegate(
        Pointer<MinecraftCommands> @this,
        Reference<ItemRegistryRef> a1,
        Reference<IWorldRegistriesProvider> a2,
        Reference<ActorFactory> a3,
        Reference<Experiments> a4,
        Reference<BaseGameVersion> a5);

    public override HookDelegate HookedFunc =>
        (@this, a2, a3, a4, a5, a6) =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(MinecraftCommandsHook));
            Global.MinecraftCommands.Instance = @this.Target;
            Original(@this, a2, a3, a4, a5, a6);
            TryUninstall();
        };
}