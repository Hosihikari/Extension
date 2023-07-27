using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class MinecraftCommandsHook : HookBase<MinecraftCommandsHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(void* @this, void* a2, void* a3, void* a4, void* a5, void* a6);

    public MinecraftCommandsHook()
        : base(
            "_ZN17MinecraftCommands13initCoreEnumsE15ItemRegistryRefRK24IWorldRegistriesProviderRK12ActorFactoryRK11ExperimentsRK15BaseGameVersion") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3, a4, a5, a6) =>
        {
            Log.Logger.Trace(nameof(MinecraftCommandsHook));
            Global.MinecraftCommands.Instance = new MinecraftCommands(@this);
            Original(@this, a2, a3, a4, a5, a6);
            TryUninstall();
        };
}