namespace Hosihikari.Minecraft.Extension.Events;

public abstract class EventArgsBase : EventArgs;

public abstract class CancelableEventArgsBase : EventArgsBase
{
    private bool _complete = true;
    public bool IsCanceled { get; private set; }

    internal void SetComplete()
    {
        _complete = true;
    }

    public void Cancel()
    {
        if (_complete)
        {
            throw new InvalidOperationException("please use before-event to cancel.");
        }

        IsCanceled = true;
    }
}