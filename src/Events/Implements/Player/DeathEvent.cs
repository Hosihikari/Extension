using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class DeathEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

//todo allow to cancel
//the effect is not prevent death, but keep inventory and exp
//Actor::getIsExperienceDropEnabled
public class DeathEvent : HookEventBase<DeathEventArgs, DeathEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr, Reference<ActorDamageSource> damageSource);

    public DeathEvent()
        : base(ServerPlayer.Original.Die) { }

    public override unsafe HookDelegate HookedFunc =>
        (serverPlayerPtr, damageSource) =>
        {
            var needCallOriginal = true;
            try
            {
                var e = new DeathEventArgs { ServerPlayer = serverPlayerPtr.Target };
                OnEventBefore(e);
                needCallOriginal = false;
                Original(serverPlayerPtr, damageSource);
                OnEventAfter(e);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(InitializedEvent), ex);
                if (needCallOriginal)
                    Original(serverPlayerPtr, damageSource);
            }
        };
}
