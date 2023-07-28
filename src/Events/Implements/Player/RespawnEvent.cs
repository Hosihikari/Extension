using Hosihikari.Minecraft.Extension.Events;
using Hosihikari.Minecraft;
using Hosihikari.NativeInterop.Hook.ObjectOriented;

public class RespawnEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player
{
    public class RespawnEvent : HookEventBase<RespawnEventArgs, RespawnEvent.HookDelegate>
    {
        public unsafe delegate void HookDelegate(void* serverPlayerPtr);

        public RespawnEvent()
            : base("?respawn@ServerPlayer@@UEAAXXZ") { }

        public override unsafe HookDelegate HookedFunc =>
            serverPlayerPtr =>
            {
                try
                { //Actor::getIsExperienceDropEnabled
                    var e = new RespawnEventArgs { ServerPlayer = new ServerPlayer(serverPlayerPtr) };
                    OnEventBefore(e);
                    Original(serverPlayerPtr);
                    OnEventAfter(e);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(nameof(InitializedEvent), ex);
                }
            };
    }
}
