using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class MinecraftHook : HookBase<MinecraftHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(Pointer<Minecraft> @this);

    public MinecraftHook()
        : base(Minecraft.Original.InitAsDedicatedServer) { }

    public override unsafe HookDelegate HookedFunc =>
        @this =>
        {
            Log.Logger.Trace(nameof(MinecraftHook));
            Original(@this);
            Global.Minecraft.Instance = new Minecraft(@this);
            TryUninstall();
        };
}