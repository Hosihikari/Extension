namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class InitializedEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

public class InitializedEvent : HookEventBase<InitializedEventArgs, InitializedEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(void* serverPlayerPtr);

    public InitializedEvent()
        : base("_ZN12ServerPlayer27setLocalPlayerAsInitializedEv") { }

    public override unsafe HookDelegate HookedFunc =>
        (serverPlayerPtr) =>
        {
            try
            {
                var e = new InitializedEventArgs
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
