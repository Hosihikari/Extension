using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class JoinEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

public class JoinEvent : HookEventBase<JoinEventArgs, JoinEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(
        Pointer<ServerNetworkHandler> serverNetworkHandler,
        Reference<NetworkIdentifier> a1,
        Reference<ConnectionRequest> a2,
        Reference<ServerPlayer> a3
    );

    public JoinEvent()
        : base(ServerNetworkHandler.Original.SendLoginMessageLocal)
    { }

    public override unsafe HookDelegate HookedFunc =>
        (handler, identifier, request, serverPlayerPtr) =>
        {
            try
            {
                var e = new JoinEventArgs { ServerPlayer = serverPlayerPtr.Target };
                OnEventBefore(e);
                Original(handler, identifier, request, serverPlayerPtr);
                OnEventAfter(e);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(InitializedEvent), ex);
            }
        };
}
