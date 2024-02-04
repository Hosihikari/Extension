using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public sealed class LeftEventArgs : EventArgsBase
{
    internal LeftEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public sealed class LeftEvent() : HookEventBase<LeftEventArgs, LeftEvent.HookDelegate>(ServerPlayer.Original.Disconnect)
{
    public delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr);

    public override HookDelegate HookedFunc =>
        serverPlayerPtr =>
        {
            LeftEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr);
            OnEventAfter(e);
        };
}