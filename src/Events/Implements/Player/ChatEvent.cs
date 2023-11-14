using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class ChatEventArgs : CancelableEventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
    public required string Message { get; init; }
}

public class ChatEvent : HookCancelableEventBase<ChatEventArgs, ChatEvent.HookDelegate>
{
    public ChatEvent()
        : base(ServerNetworkHandler.Original.Handle) { }

    public unsafe delegate void HookDelegate(
        Pointer<ServerNetworkHandler> networkHandler,
        Reference<NetworkIdentifier> networkIdentifier,
        Reference<TextPacket> textPacket
    );

    public override unsafe HookDelegate HookedFunc =>
        (networkHandlerPtr, networkIdentifierPtr, textPacketPtr) =>
        {
            try
            {
                var networkHandler = networkHandlerPtr.Target;
                var networkIdentifier = networkIdentifierPtr.Target;
                var packet = textPacketPtr.Target;

                throw new NotImplementedException();

                //if (networkHandler.TryFetchPlayer(networkIdentifier, packet, out var player))
                //{
                //    var textType = *((byte*)textPacketPtr + OffsetData.Current.TextPacketTextTypeOffsetByte);//44
                //    if (textType != 1)//not a chat packet
                //    {
                //        Original(networkHandlerPtr, networkIdentifierPtr, textPacketPtr);
                //        return;
                //    }
                //    var e = new ChatEventArgs
                //    {
                //        ServerPlayer = player,
                //        Message = NativeInterop.Utils.StringUtils.MarshalStdString((byte*)textPacketPtr + OffsetData.Current.TextPacketMessageOffsetByte)//80
                //    };
                //    OnEventBefore(e);
                //    if (e.IsCanceled)
                //        return; //cancel the original
                //    Original(networkHandlerPtr, networkIdentifierPtr, textPacketPtr);
                //    OnEventAfter(e);
                //}
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(ChatEvent), ex);
            }
        };
}