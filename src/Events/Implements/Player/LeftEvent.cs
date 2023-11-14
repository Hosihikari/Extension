using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class LeftEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

public class LeftEvent : HookEventBase<LeftEventArgs, LeftEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr);

    public LeftEvent()
        : base(ServerPlayer.Original.Disconnect) { }

    public override unsafe HookDelegate HookedFunc =>
        serverPlayerPtr =>
        {
            try
            {
                var e = new LeftEventArgs { ServerPlayer = serverPlayerPtr.Target };
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
