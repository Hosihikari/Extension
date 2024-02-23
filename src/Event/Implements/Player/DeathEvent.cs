using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Event.Implements.Player;

public sealed class DeathEventArgs : EventArgsBase
{
    internal DeathEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

//todo allow to cancel
//the effect is not prevent death, but keep inventory and exp
//Actor::getIsExperienceDropEnabled
public sealed class DeathEvent() : HookEventBase<DeathEventArgs, DeathEvent.HookDelegateType>(ServerPlayer.Original.Die)
{
    public delegate void HookDelegateType(Pointer<ServerPlayer> serverPlayerPtr,
        Reference<ActorDamageSource> damageSource);

    protected override HookDelegateType HookDelegate =>
        (serverPlayerPtr, damageSource) =>
        {
            DeathEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr, damageSource);
            OnEventAfter(e);
        };
}