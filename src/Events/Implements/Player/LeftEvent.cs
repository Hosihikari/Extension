namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class LeftEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

public class LeftEvent : HookEventBase<LeftEventArgs, LeftEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(void* serverPlayerPtr);

    public LeftEvent() : base(
        "_ZN12ServerPlayer10disconnectEv") { }

    public override unsafe HookDelegate HookedFunc => (serverPlayerPtr) =>
    {
        try
        {
            var e = new LeftEventArgs()
            {
                ServerPlayer = new ServerPlayer(serverPlayerPtr)
            };
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