using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Event.Implements.Player;

public sealed class InitializedEventArgs : EventArgsBase
{
    internal InitializedEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public sealed class InitializedEvent()
    : HookEventBase<InitializedEventArgs, InitializedEvent.HookDelegateType>(ServerPlayer.Original
        .SetLocalPlayerAsInitialized)
{
    public delegate void HookDelegateType(Pointer<ServerPlayer> serverPlayerPtr);

    protected override HookDelegateType HookDelegate =>
        serverPlayerPtr =>
        {
            InitializedEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr);
            OnEventAfter(e);
        };
}