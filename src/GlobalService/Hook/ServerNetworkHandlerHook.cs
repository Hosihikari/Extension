using System.Runtime.InteropServices;
using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class ServerNetworkHandlerHook : HookBase<ServerNetworkHandlerHook.HookDelegate>
{
    internal unsafe delegate void HookDelegate(void* @this, void* str, [MarshalAs(UnmanagedType.U1)] bool a2);

    public ServerNetworkHandlerHook()
        : base(
            "_ZN20ServerNetworkHandler24allowIncomingConnectionsERKNSt7__cxx1112basic_stringIcSt11char_traitsIcESaIcEEEb") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, str, a2) =>
        {
            Log.Logger.Trace(nameof(ServerNetworkHandlerHook));
            Original(@this, str, a2);
            Global.ServerNetworkHandler.Instance = new ServerNetworkHandler(@this);
            TryUninstall();
        };
}