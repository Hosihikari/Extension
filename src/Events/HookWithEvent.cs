using Hosihikari.Core;
using NativeInterop.Hook.OOP;

namespace Hosihikari.Minecraft.Events;

public abstract class HookWithEvent<TEventArgs, TEvent, THookDelegate> : HookBase<THookDelegate>
    where TEventArgs : HosihikariEventArgs
    where TEvent : HosihikariEventBase<TEventArgs>, new()
    where THookDelegate : Delegate
{
    protected HookWithEvent(string symbol)
        : base(symbol) { }

    protected HookWithEvent(IntPtr address)
        : base(address) { }

    public TEvent Event { get; } = new();
}

public abstract class HookWithCancelableEvent<TEventArgs> : HosihikariEventBase<TEventArgs>
    where TEventArgs : HosihikariCancelableEventArgs
{
    public HookWithCancelableEvent(string symbol) { }
}
