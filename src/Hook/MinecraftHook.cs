using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Shared.Hook;

internal sealed class MinecraftHook()
    : HookBase<MinecraftHook.HookDelegateType>(Minecraft.Original.InitAsDedicatedServer)
{
    protected override HookDelegateType HookDelegate =>
        @this =>
        {
            Original(@this);
            Global.Minecraft.Instance = new(@this);
            TryUninstall();
        };

    internal delegate void HookDelegateType(Pointer<Minecraft> @this);
}