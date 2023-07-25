using Hosihikari.Core;

namespace Hosihikari.Minecraft.Events;

public class HookEventBase<T, TEventArgs, TEvent, THookDelegate> : HosihikariEventBase<T>
    where T : HosihikariEventArgs
    where TEventArgs : HosihikariEventArgs
    where TEvent : HosihikariEventBase<TEventArgs>, new()
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
