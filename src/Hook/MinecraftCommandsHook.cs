using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Shared.Hook;

internal sealed class MinecraftCommandsHook()
    : HookBase<MinecraftCommandsHook.HookDelegateType>(MinecraftCommands.Original.InitCoreEnums)
{
    protected override HookDelegateType HookDelegate =>
        (@this, a2, a3, a4, a5, a6) =>
        {
            Global.MinecraftCommands.Instance = @this.Target;
            Original(@this, a2, a3, a4, a5, a6);
            TryUninstall();
        };

    internal delegate void HookDelegateType(
        Pointer<MinecraftCommands> @this,
        Reference<ItemRegistryRef> a1,
        Reference<IWorldRegistriesProvider> a2,
        Reference<ActorFactory> a3,
        Reference<Experiments> a4,
        Reference<BaseGameVersion> a5);
}