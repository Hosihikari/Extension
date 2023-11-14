using static Hosihikari.Minecraft.RakNet;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class RakPeerHook : HookBase<RakPeerHook.HookDelegate>
{
    internal unsafe delegate Pointer<RakPeer> HookDelegate(Pointer<RakPeer> @this);

    public RakPeerHook()
        : base(RakPeer.Original.Constructor_RakPeer)
    { }

    public override unsafe HookDelegate HookedFunc =>
        @this =>
        {
            Log.Logger.Trace(nameof(RakPeerHook));
            var result = Original(@this);
            Global.RakPeer.Instance = @this.Target;
            TryUninstall();
            return result;
        };
}