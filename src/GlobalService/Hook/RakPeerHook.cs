using Hosihikari.Minecraft.RakNet;
using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class RakPeerHook : HookBase<RakPeerHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(void* @this);

    public RakPeerHook()
        : base("_ZN6RakNet7RakPeerC2Ev") { }

    public override unsafe HookDelegate HookedFunc =>
        @this =>
        {
            Log.Logger.Trace(nameof(RakPeerHook));
            var result = Original(@this);
            Global.RakPeer.Instance = new RakPeer(@this);
            TryUninstall();
            return result;
        };
}