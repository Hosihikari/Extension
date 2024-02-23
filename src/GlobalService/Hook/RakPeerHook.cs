using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class RakPeerHook() : HookBase<RakPeerHook.HookDelegateType>(RakPeer.Original.Constructor_RakPeer)
{
    protected override HookDelegateType HookDelegate =>
        @this =>
        {
            Pointer<RakPeer> result = Original(@this);
            Global.RakPeer.Instance = @this.Target;
            TryUninstall();
            return result;
        };

    internal delegate Pointer<RakPeer> HookDelegateType(Pointer<RakPeer> @this);
}