using Hosihikari.Minecraft.Extension.Events.Implements.Player;

namespace Hosihikari.Minecraft.Extension.Events;

public static class Events
{
    private static readonly Lazy<ChatEvent> ChatEventHook = new(() => new());
    public static ChatEvent Chat => ChatEventHook.Value;
}
