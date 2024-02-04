using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

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
            try
            {
                LeftEventArgs e = new(serverPlayerPtr.Target);
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