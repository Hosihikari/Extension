using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public sealed class JoinEventArgs : EventArgsBase
{
    internal JoinEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public sealed class JoinEvent()
    : HookEventBase<JoinEventArgs, JoinEvent.HookDelegate>(ServerNetworkHandler.Original.SendLoginMessageLocal)
{
    public delegate void HookDelegate(
        Pointer<ServerNetworkHandler> serverNetworkHandler,
        Reference<NetworkIdentifier> a1,
        Reference<ConnectionRequest> a2,
        Reference<ServerPlayer> a3
    );

    public override HookDelegate HookedFunc =>
        (handler, identifier, request, serverPlayerPtr) =>
        {
            JoinEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(handler, identifier, request, serverPlayerPtr);
            OnEventAfter(e);
        };
}