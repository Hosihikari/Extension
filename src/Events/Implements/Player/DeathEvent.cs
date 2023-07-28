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
    public unsafe delegate void HookDelegate(void* serverPlayerPtr, void* damageSource);

    public DeathEvent()
        : base("?die@ServerPlayer@@UEAAXAEBVActorDamageSource@@@Z") { }

    public override unsafe HookDelegate HookedFunc =>
        (serverPlayerPtr, damageSource) =>
        {
            try
            {
                var e = new DeathEventArgs { ServerPlayer = new ServerPlayer(serverPlayerPtr) };
                OnEventBefore(e);
                Original(serverPlayerPtr, damageSource);
                OnEventAfter(e);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(InitializedEvent), ex);
            }
        };
}
