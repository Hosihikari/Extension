// namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;
//
// public class ChatEventHook : HookWithEvent<ChatEventArgs, ChatEvent, ChatEventHook.HookDelegate>
// {
//     public ChatEventHook()
//         : base("todo") { }
//
//     public delegate void HookDelegate();
//
//     public override HookDelegate HookedFunc =>
//         () =>
//         {
//             var e = new ChatEventArgs();
//             Event.OnEventBefore(e);
//             if (e.IsCanceled)
//                 return;
//             Original();
//             Event.OnEventAfter(e);
//         };
// }
//
// public class ChatEventArgs : CancelableEventArgs { }
//
// public class ChatEvent : EventBase<ChatEventArgs>
// {
//     public override void BeforeEventAdded()
//     {
//         //todo install hook when first event added
//     }
//
//     public override void OnEventAllRemoved()
//     {
//         //todo uninstall hook when all event removed
//     }
// }
