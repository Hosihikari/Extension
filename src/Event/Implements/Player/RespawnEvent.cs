using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Event.Implements.Player;

public sealed class RespawnEventArgs : EventArgsBase
{
    internal RespawnEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public class RespawnEvent()
    : HookEventBase<RespawnEventArgs, RespawnEvent.HookDelegateType>(ServerPlayer.Original.Respawn)
{
    public delegate void HookDelegateType(Pointer<ServerPlayer> serverPlayerPtr);

    protected override HookDelegateType HookDelegate =>
        serverPlayerPtr =>
        {
            //Actor::getIsExperienceDropEnabled
            RespawnEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr);
            OnEventAfter(e);
        };
}