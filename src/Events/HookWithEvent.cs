using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.Events;

public abstract class HookWithEvent<TEventArgs, TEvent, THookDelegate> : HookBase<THookDelegate>
    where TEventArgs : EventArgs
    where TEvent : EventBase<TEventArgs>, new()
    where THookDelegate : Delegate
{
    protected HookWithEvent(string symbol)
        : base(symbol) { }

    protected HookWithEvent(IntPtr address)
        : base(address) { }

    public TEvent Event { get; } = new();
}

public abstract class HookWithCancelableEvent<TEventArgs> : EventBase<TEventArgs>
    where TEventArgs : CancelableEventArgs
{
    public HookWithCancelableEvent(string symbol) { }
}
