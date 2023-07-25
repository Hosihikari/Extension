namespace Hosihikari.Minecraft.Extension.Events;

public class HookEventBase<T, TEventArgs, TEvent, THookDelegate> : EventBase<T>
    where T : EventArgs
    where TEventArgs : EventArgs
    where TEvent : EventBase<TEventArgs>, new()
    where THookDelegate : Delegate
{
    public override void BeforeEventAdded()
    {
        //todo install hook when first event added
    }

    public override void OnEventAllRemoved()
    {
        //todo uninstall hook when all event removed
    }
}
