namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class ChatEventArgs : CancelableEventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
    public required string Message { get; init; }
}

public class ChatEvent : HookCancelableEventBase<ChatEventArgs, ChatEvent.HookDelegate>
{
    public ChatEvent()
        : base("_ZN20ServerNetworkHandler6handleERK17NetworkIdentifierRK10TextPacket") { }

    public unsafe delegate void HookDelegate(
        void* networkHandler,
        void* networkIdentifier,
        void* textPacket
    );

    public override unsafe HookDelegate HookedFunc =>
        (networkHandlerPtr, networkIdentifierPtr, textPacketPtr) =>
        { 
            try
            {
                var networkHandler = new ServerNetworkHandler(networkHandlerPtr);
                var networkIdentifier = new NetworkIdentifier(networkIdentifierPtr);
                var packet = new Packet(textPacketPtr);
                if (networkHandler.TryFetchPlayer(networkIdentifier, packet, out var player))
                {
                    var textType = *((byte*)textPacketPtr + OffsetData.Current.TextPacketTextTypeOffsetByte);//44
                    if (textType != 1)//not a chat packet
                    {
                        Original(networkHandlerPtr, networkIdentifierPtr, textPacketPtr);
                        return;
                    }
                    var e = new ChatEventArgs
                    {
                        ServerPlayer = player,
                        Message = NativeInterop.Utils.StringUtils.MarshalStdString((byte*)textPacketPtr + OffsetData.Current.TextPacketMessageOffsetByte)//80
                    };
                    OnEventBefore(e);
                    if (e.IsCanceled)
                        return; //cancel the original
                    Original(networkHandlerPtr, networkIdentifierPtr, textPacketPtr);
                    OnEventAfter(e);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(ChatEvent), ex);
            }
        };
}