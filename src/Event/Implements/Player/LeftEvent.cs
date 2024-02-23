using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Event.Implements.Player;

public sealed class LeftEventArgs : EventArgsBase
{
    internal LeftEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public sealed class LeftEvent()
    : HookEventBase<LeftEventArgs, LeftEvent.HookDelegateType>(ServerPlayer.Original.Disconnect)
{
    public delegate void HookDelegateType(Pointer<ServerPlayer> serverPlayerPtr);

    protected override HookDelegateType HookDelegate =>
        serverPlayerPtr =>
        {
            LeftEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr);
            OnEventAfter(e);
        };
}