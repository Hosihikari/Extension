using System.Runtime.InteropServices;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Hosihikari.NativeInterop.Unmanaged.STL;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class ServerNetworkHandlerHook : HookBase<ServerNetworkHandlerHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(Pointer<ServerNetworkHandler> @this, Reference<StdString> str, [MarshalAs(UnmanagedType.U1)] bool a2);

    public ServerNetworkHandlerHook()
        : base(ServerNetworkHandler.Original.AllowIncomingConnections)
    { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, str, a2) =>
        {
            Log.Logger.Trace(nameof(ServerNetworkHandlerHook));
            Original(@this, str, a2);
            Global.ServerNetworkHandler.Instance = @this.Target;
            TryUninstall();
        };
}