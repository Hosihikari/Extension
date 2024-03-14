using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Hosihikari.NativeInterop.Unmanaged.STL;
using System.Runtime.InteropServices;

namespace Hosihikari.Minecraft.Extension.Shared.Hook;

internal sealed class ServerNetworkHandlerHook()
    : HookBase<ServerNetworkHandlerHook.HookDelegateType>(ServerNetworkHandler.Original.AllowIncomingConnections)
{
    protected override HookDelegateType HookDelegate =>
        (@this, str, a2) =>
        {
            Original(@this, str, a2);
            Global.ServerNetworkHandler.Instance = @this.Target;
            TryUninstall();
        };

    internal delegate void HookDelegateType(Pointer<ServerNetworkHandler> @this, StdString str,
        [MarshalAs(UnmanagedType.U1)] bool a2);
}