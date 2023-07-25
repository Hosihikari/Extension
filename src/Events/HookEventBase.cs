using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.Events;

public abstract class HookEventBase<TEventArgs, THookDelegate> : HookBase<THookDelegate>
    where TEventArgs : EventArgs
    where THookDelegate : Delegate
{
    private event EventHandler<TEventArgs>? InternalBefore;
    private event EventHandler<TEventArgs>? InternalAfter;
    private event EventHandler<TEventArgs>? InternalAsync;

    public event EventHandler<TEventArgs> Before
    {
        add
        {
            CheckEventAdded();
            InternalBefore += value;
        }
        remove
        {
            InternalBefore -= value;
            CheckEventRemoved();
        }
    }

    private void CheckEventAdded()
    {
        if (InternalBefore is null && InternalAfter is null && InternalAsync is null)
            BeforeEventAdded();
    }

    private void CheckEventRemoved()
    {
        if (InternalBefore is null && InternalAfter is null && InternalAsync is null)
            OnEventAllRemoved();
    }

    protected virtual void BeforeEventAdded()
    {
        //todo install hook when first event added
    }

    protected virtual void OnEventAllRemoved()
    {
        //todo uninstall hook when all event removed
    }

    protected void OnEventBefore(TEventArgs e) => InternalBefore?.Invoke(this, e);
    protected void OnEventAfter(TEventArgs e)
    {
        InternalAfter?.Invoke(this, e);
    }

    protected HookEventBase(string symbol) : base(symbol) { }
}