using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Event.Implements.Player;

public sealed class ChatEventArgs : CancelableEventArgsBase
{
    internal ChatEventArgs(ServerPlayer serverPlayer, string message)
    {
        ServerPlayer = serverPlayer;
        Message = message;
    }

    public ServerPlayer ServerPlayer { get; }
    public string Message { get; }
}

public sealed class ChatEvent()
    : HookCancelableEventBase<ChatEventArgs, ChatEvent.HookDelegateType>(ServerNetworkHandler.Original.Handle)
{
    public delegate void HookDelegateType(
        Pointer<ServerNetworkHandler> networkHandler,
        Reference<NetworkIdentifier> networkIdentifier,
        Reference<TextPacket> textPacket
    );

    protected override HookDelegateType HookDelegate =>
        (networkHandlerPtr, networkIdentifierPtr, textPacketPtr) =>
        {
            ServerNetworkHandler networkHandler = networkHandlerPtr.Target;
            NetworkIdentifier networkIdentifier = networkIdentifierPtr.Target;
            TextPacket packet = textPacketPtr.Target;

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
        };
}