using Hosihikari.Minecraft.Events.Implements.Player;

namespace Hosihikari.Minecraft.Events;

public static class Events
{
    private static readonly ChatEventHook ChatEventHook = new();
    public static ChatEvent Chat => ChatEventHook.Event;
}
