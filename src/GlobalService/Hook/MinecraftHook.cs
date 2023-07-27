using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class MinecraftHook : HookBase<MinecraftHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(void* @this, void* a2);

    public MinecraftHook()
        : base("_ZN9Minecraft21initAsDedicatedServerEv") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2) =>
        {
            Log.Logger.Trace(nameof(MinecraftHook));
            Original(@this, a2);
            Global.Minecraft.Instance = new Minecraft(@this);
            TryUninstall();
        };
}