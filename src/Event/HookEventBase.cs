using Hosihikari.NativeInterop.Hook.ObjectOriented;
using System.Runtime.CompilerServices;

namespace Hosihikari.Minecraft.Extension.Event;

public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs e)
    where TEventArgs : EventArgsBase;

public abstract class HookEventBase<TEventArgs, THookDelegate> : HookBase<THookDelegate>
    where TEventArgs : EventArgsBase
    where THookDelegate : Delegate
{
    private readonly string _className;

    protected HookEventBase(string symbol, [CallerFilePath] string sourceFile = "")
        : base(symbol)
    {
        _className = System.IO.Path.GetFileNameWithoutExtension(
            sourceFile.Replace("\\", "/") /*fix if compile in windows*/
        );
    }

    protected HookEventBase(Delegate func, [CallerFilePath] string sourceFile = "")
        : base(func)
    {
        _className = System.IO.Path.GetFileNameWithoutExtension(
            sourceFile.Replace("\\", "/") /*fix if compile in windows*/
        );
    }

    private event EventHandler<TEventArgs>? InternalBefore;
    private event EventHandler<TEventArgs>? InternalAfter;
    private event AsyncEventHandler<TEventArgs>? InternalAsync;

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

    public event EventHandler<TEventArgs> After
    {
        add
        {
            CheckEventAdded();
            InternalAfter += value;
        }
        remove
        {
            InternalAfter -= value;
            CheckEventRemoved();
        }
    }

    public event AsyncEventHandler<TEventArgs> Async
    {
        add
        {
            CheckEventAdded();
            InternalAsync += value;
        }
        remove
        {
            InternalAsync -= value;
            CheckEventRemoved();
        }
    }

    private void CheckEventAdded()
    {
        if (InternalBefore is null && InternalAfter is null && InternalAsync is null)
        {
            LevelTick.PostTick(BeforeEventAdded);
        }
    }

    private void CheckEventRemoved()
    {
        if (InternalBefore is null && InternalAfter is null && InternalAsync is null)
        {
            LevelTick.PostTick(AfterEventAllRemoved);
        }
    }

    protected virtual void BeforeEventAdded()
    {
        //install hook when first event added
        if (!HasInstalled)
        {
            Install();
        }
    }

    protected virtual void AfterEventAllRemoved()
    {
        //uninstall hook when all event removed
        if (HasInstalled)
        {
            Uninstall();
        }
    }

    protected virtual void OnEventBefore(TEventArgs e)
    {
        InternalBefore?.Invoke(this, e);
    }

    protected virtual void OnEventAfter(TEventArgs e)
    {
        InternalAfter?.Invoke(this, e);
        Task? task = InternalAsync?.Invoke(this, e);
        //todo allow user to toggle off in config ?
        //output error when async event throw exception
        task?.ContinueWith(
            t => Environment.FailFast($"Unhandled Exception in {_className + "::" + nameof(OnEventAfter) + "Async"}",
                t.Exception),
            TaskContinuationOptions.OnlyOnFaulted
        );
    }
}

public abstract class HookCancelableEventBase<TEventArgs, THookDelegate>
    : HookEventBase<TEventArgs, THookDelegate>
    where TEventArgs : CancelableEventArgsBase
    where THookDelegate : Delegate
{
    protected HookCancelableEventBase(string symbol, [CallerFilePath] string sourceFile = "")
        : base(symbol, sourceFile)
    {
    }

    protected HookCancelableEventBase(Delegate func, [CallerFilePath] string sourceFile = "")
        : base(func, sourceFile)
    {
    }

    protected override void OnEventAfter(TEventArgs e)
    {
        //if the event canceled in before-event,the will not pass to after-event
        if (e.IsCanceled)
        {
            return;
        }

        base.OnEventAfter(e);
    }
}