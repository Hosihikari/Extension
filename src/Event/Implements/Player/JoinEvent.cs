using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Event.Implements.Player;

public sealed class JoinEventArgs : EventArgsBase
{
    internal JoinEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public sealed class JoinEvent()
    : HookEventBase<JoinEventArgs, JoinEvent.HookDelegateType>(ServerNetworkHandler.Original.SendLoginMessageLocal)
{
    public delegate void HookDelegateType(
        Pointer<ServerNetworkHandler> serverNetworkHandler,
        Reference<NetworkIdentifier> a1,
        Reference<ConnectionRequest> a2,
        Reference<ServerPlayer> a3
    );

    protected override HookDelegateType HookDelegate =>
        (handler, identifier, request, serverPlayerPtr) =>
        {
            JoinEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(handler, identifier, request, serverPlayerPtr);
            OnEventAfter(e);
        };
}