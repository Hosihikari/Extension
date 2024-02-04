using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class MinecraftHook() : HookBase<MinecraftHook.HookDelegate>(Minecraft.Original.InitAsDedicatedServer)
{
    public override HookDelegate HookedFunc =>
        @this =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(MinecraftHook));
            Original(@this);
            Global.Minecraft.Instance = new(@this);
            TryUninstall();
        };

    internal delegate void HookDelegate(Pointer<Minecraft> @this);
}