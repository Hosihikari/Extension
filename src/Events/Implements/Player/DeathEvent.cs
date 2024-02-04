using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

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
public sealed class DeathEvent() : HookEventBase<DeathEventArgs, DeathEvent.HookDelegate>(ServerPlayer.Original.Die)
{
    public delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr, Reference<ActorDamageSource> damageSource);

    public override HookDelegate HookedFunc =>
        (serverPlayerPtr, damageSource) =>
        {
            DeathEventArgs e = new(serverPlayerPtr.Target);
            OnEventBefore(e);
            Original(serverPlayerPtr, damageSource);
            OnEventAfter(e);
        };
}