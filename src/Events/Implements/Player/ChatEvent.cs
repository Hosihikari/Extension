using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

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
    : HookCancelableEventBase<ChatEventArgs, ChatEvent.HookDelegate>(ServerNetworkHandler.Original.Handle)
{
    public delegate void HookDelegate(
        Pointer<ServerNetworkHandler> networkHandler,
        Reference<NetworkIdentifier> networkIdentifier,
        Reference<TextPacket> textPacket
    );

    public override HookDelegate HookedFunc =>
        (networkHandlerPtr, networkIdentifierPtr, textPacketPtr) =>
        {
            try
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
            }
            catch (Exception ex)
            {
                Log.Logger.LogError("Unhandled Exception in {ModuleName}: {Exception}", nameof(ChatEvent), ex);
            }
        };
}