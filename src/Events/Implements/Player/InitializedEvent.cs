using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public sealed class InitializedEventArgs : EventArgsBase
{
    internal InitializedEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public sealed class InitializedEvent()
    : HookEventBase<InitializedEventArgs, InitializedEvent.HookDelegate>(ServerPlayer.Original
        .SetLocalPlayerAsInitialized)
{
    public delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr);

    public override HookDelegate HookedFunc =>
        serverPlayerPtr =>
        {
            InitializedEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr);
            OnEventAfter(e);
        };
}