using Hosihikari.Minecraft.Extension.Events.Implements.Player;

namespace Hosihikari.Minecraft.Extension.Events;

public static class Events
{
    private static readonly Lazy<ChatEvent> PlayerChatEvent = new(() => new());
    public static ChatEvent PlayerChat => PlayerChatEvent.Value;

    private static readonly Lazy<InitializedEvent> PlayerInitializedEvent = new(() => new());
    public static InitializedEvent PlayerInitialized => PlayerInitializedEvent.Value;

    private static readonly Lazy<JoinEvent> PlayerJoinEvent = new(() => new());
    public static JoinEvent PlayerJoin => PlayerJoinEvent.Value;
    private static readonly Lazy<LeftEvent> PlayerLeftEvent = new(() => new());
    public static LeftEvent PlayerLeft => PlayerLeftEvent.Value;
    private static readonly Lazy<RespawnEvent> PlayerRespawnEvent = new(() => new());
    public static RespawnEvent PlayerRespawn => PlayerRespawnEvent.Value;
    private static readonly Lazy<DeathEvent> PlayerDeathEvent = new(() => new());
    public static DeathEvent PlayerDeath => PlayerDeathEvent.Value;
}
