namespace Hosihikari.Minecraft.Extension.Events;

public static class Events
{
    private static readonly ChatEventHook ChatEventHook = new();
    public static ChatEvent Chat => ChatEventHook.Event;
}
