namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class JoinEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

public class JoinEvent : HookEventBase<JoinEventArgs, JoinEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(void* serverNetworkHandler,
        void* networkIdentifier,
        void* connectionRequest, void* serverPlayerPtr);

    public JoinEvent() : base(
        "_ZN20ServerNetworkHandler21sendLoginMessageLocalERK17NetworkIdentifierRK17ConnectionRequestR12ServerPlayer") { }

    public override unsafe HookDelegate HookedFunc => (handler, identifier, request, serverPlayerPtr) =>
    {
        try
        {
            var e = new JoinEventArgs()
            {
                ServerPlayer = new ServerPlayer(serverPlayerPtr)
            };
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