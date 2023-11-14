using Hosihikari.Minecraft.Extension.Events;
using Hosihikari.Minecraft;
using Hosihikari.NativeInterop.Unmanaged;

public class RespawnEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player
{
    public class RespawnEvent : HookEventBase<RespawnEventArgs, RespawnEvent.HookDelegate>
    {
        public unsafe delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr);

        public RespawnEvent()
            : base(ServerPlayer.Original.Respawn) { }

        public override unsafe HookDelegate HookedFunc =>
            serverPlayerPtr =>
            {
                try
                { //Actor::getIsExperienceDropEnabled
                    var e = new RespawnEventArgs { ServerPlayer = serverPlayerPtr.Target };
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
