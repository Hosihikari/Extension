namespace Hosihikari.Minecraft.Extension.Events;

public abstract class EventArgsBase : EventArgs { }

public abstract class CancelableEventArgs : EventArgsBase
{
    public bool IsCanceled { get; private set; }
    private bool _complete = true;
    internal void SetComplete() => _complete = true;
    public void Cancel()
    {
        if (_complete)
            throw new InvalidOperationException("please use before-event to cancel.");
        IsCanceled = true;
    }
}