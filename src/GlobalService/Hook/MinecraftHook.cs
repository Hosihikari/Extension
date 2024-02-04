using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class MinecraftHook() : HookBase<MinecraftHook.HookDelegate>(Minecraft.Original.InitAsDedicatedServer)
{
    public override HookDelegate HookedFunc =>
        @this =>
        {
            Original(@this);
            Global.Minecraft.Instance = new(@this);
            TryUninstall();
        };

    internal delegate void HookDelegate(Pointer<Minecraft> @this);
}