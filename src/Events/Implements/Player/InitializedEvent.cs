using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class InitializedEventArgs : EventArgsBase
{
    public required ServerPlayer ServerPlayer { get; init; }
}

public class InitializedEvent : HookEventBase<InitializedEventArgs, InitializedEvent.HookDelegate>
{
    public unsafe delegate void HookDelegate(Pointer<ServerPlayer> serverPlayerPtr);

    public InitializedEvent()
        : base(ServerPlayer.Original.SetLocalPlayerAsInitialized) { }

    public override unsafe HookDelegate HookedFunc =>
        (serverPlayerPtr) =>
        {
            try
            {
                var e = new InitializedEventArgs { ServerPlayer = serverPlayerPtr.Target };
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
