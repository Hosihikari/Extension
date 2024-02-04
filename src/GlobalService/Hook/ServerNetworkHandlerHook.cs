using System.Runtime.InteropServices;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Hosihikari.NativeInterop.Unmanaged.STL;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class ServerNetworkHandlerHook()
    : HookBase<ServerNetworkHandlerHook.HookDelegate>(ServerNetworkHandler.Original.AllowIncomingConnections)
{
    internal delegate void HookDelegate(Pointer<ServerNetworkHandler> @this, StdString str, [MarshalAs(UnmanagedType.U1)] bool a2);

    public override unsafe HookDelegate HookedFunc =>
        (@this, str, a2) =>
        {
            Log.Logger.LogTrace("In {ModuleName}", nameof(ServerNetworkHandlerHook));
            Original(@this, str, a2);
            Global.ServerNetworkHandler.Instance = @this.Target;
            TryUninstall();
        };
}