namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class ChatEventArgs : CancelableEventArgs
{
    public required ServerPlayer Player { get; init; }
}

public class ChatEvent : HookEventBase<ChatEventArgs, ChatEvent.HookDelegate>
{
    public ChatEvent()
        : base("_ZN20ServerNetworkHandler6handleERK17NetworkIdentifierRK10TextPacket") { }

    public unsafe delegate void HookDelegate(
        void* networkHandler,
        void* networkIdentifier,
        void* textPacket
    );
    public override unsafe HookDelegate HookedFunc =>
        (networkHandlerPtr, networkIdentifierPtr, textPacket) =>
        {
            try
            {
                var networkHandler = new ServerNetworkHandler(networkHandlerPtr);
                var networkIdentifier = new NetworkIdentifier(networkIdentifierPtr);
                var packet = new Packet(textPacket);
                if (networkHandler.TryFetchPlayer(networkIdentifier, packet, out var player))
                {
                    var e = new ChatEventArgs { Player = player };
                    OnEventBefore(e);
                    if (e.IsCanceled)
                        return; //cancel the original
                    Original(networkHandlerPtr, networkIdentifierPtr, textPacket);
                    OnEventAfter(e);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(ChatEvent), ex);
            }
        };
}
