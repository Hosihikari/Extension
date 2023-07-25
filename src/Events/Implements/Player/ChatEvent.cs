using Hosihikari.Core;

namespace Hosihikari.Minecraft.Events.Implements.Player;

public class ChatEventHook : HookWithEvent<ChatEventArgs, ChatEvent, ChatEventHook.HookDelegate>
{
    public ChatEventHook()
        : base("todo") { }

    public delegate void HookDelegate();

    public override HookDelegate HookedFunc =>
        () =>
        {
            var e = new ChatEventArgs();
            Event.OnEventBefore(e);
            if (e.IsCanceled)
                return;
            Original();
            Event.OnEventAfter(e);
        };
}

public class ChatEventArgs : HosihikariCancelableEventArgs { }

public class ChatEvent : HosihikariEventBase<ChatEventArgs>
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
