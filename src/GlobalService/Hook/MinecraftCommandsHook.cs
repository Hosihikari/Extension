using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class MinecraftCommandsHook : HookBase<MinecraftCommandsHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(
        Pointer<MinecraftCommands> @this,
        Reference<ItemRegistryRef> a1,
        Reference<IWorldRegistriesProvider> a2,
        Reference<ActorFactory> a3,
        Reference<Experiments> a4,
        Reference<BaseGameVersion> a5);

    public MinecraftCommandsHook()
        : base(MinecraftCommands.Original.InitCoreEnums) { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3, a4, a5, a6) =>
        {
            Log.Logger.Trace(nameof(MinecraftCommandsHook));
            Global.MinecraftCommands.Instance = @this.Target;
            Original(@this, a2, a3, a4, a5, a6);
            TryUninstall();
        };
}