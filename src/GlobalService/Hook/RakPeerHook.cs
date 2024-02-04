using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;
using static Hosihikari.Minecraft.RakNet;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class RakPeerHook() : HookBase<RakPeerHook.HookDelegate>(RakPeer.Original.Constructor_RakPeer)
{
    public override HookDelegate HookedFunc =>
        @this =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(RakPeerHook));
            Pointer<RakPeer> result = Original(@this);
            Global.RakPeer.Instance = @this.Target;
            TryUninstall();
            return result;
        };

    internal delegate Pointer<RakPeer> HookDelegate(Pointer<RakPeer> @this);
}