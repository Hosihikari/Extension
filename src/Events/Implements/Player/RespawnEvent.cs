using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public sealed class RespawnEventArgs : EventArgsBase
{
    internal RespawnEventArgs(ServerPlayer serverPlayer)
    {
        ServerPlayer = serverPlayer;
    }

    public ServerPlayer ServerPlayer { get; }
}

public class RespawnEvent() : HookEventBase<RespawnEventArgs, RespawnEvent.HookDelegate>(ServerPlayer.Original.Respawn)
{
    public delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr);

    public override HookDelegate HookedFunc =>
        serverPlayerPtr =>
        {
            try
            {
                //Actor::getIsExperienceDropEnabled
                RespawnEventArgs e = new(serverPlayerPtr.Target);
                OnEventBefore(e);
                Original(serverPlayerPtr);
                OnEventAfter(e);
            }
            catch (Exception ex)
            {
                Log.Logger.LogError("Unhandled Exception in {ModuleName}: {Exception}", nameof(InitializedEvent), ex);
            }
        };
}