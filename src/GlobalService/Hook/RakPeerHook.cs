using static Hosihikari.Minecraft.RakNet;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class RakPeerHook() : HookBase<RakPeerHook.HookDelegate>(RakPeer.Original.Constructor_RakPeer)
{
    internal delegate Pointer<RakPeer> HookDelegate(Pointer<RakPeer> @this);

    public override HookDelegate HookedFunc =>
        @this =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(RakPeerHook));
            Pointer<RakPeer> result = Original(@this);
            Global.RakPeer.Instance = @this.Target;
            TryUninstall();
            return result;
        };
}