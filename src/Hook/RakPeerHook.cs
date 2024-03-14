using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Shared.Hook;

internal sealed class RakPeerHook()
    : HookBase<RakPeerHook.HookDelegateType>(RakNet.RakPeer.Original.Constructor_RakPeer)
{
    protected override HookDelegateType HookDelegate =>
        @this =>
        {
            Pointer<RakNet.RakPeer> result = Original(@this);
            Global.RakPeer.Instance = @this.Target;
            TryUninstall();
            return result;
        };

    internal delegate Pointer<RakNet.RakPeer> HookDelegateType(Pointer<RakNet.RakPeer> @this);
}