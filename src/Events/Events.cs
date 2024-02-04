using Hosihikari.Minecraft.Extension.Events.Implements.Player;

namespace Hosihikari.Minecraft.Extension.Events;

public static class Events
{
    private static readonly Lazy<ChatEvent> s_playerChatEvent = new(() => new());
    public static ChatEvent PlayerChat => s_playerChatEvent.Value;

    private static readonly Lazy<InitializedEvent> s_playerInitializedEvent = new(() => new());
    public static InitializedEvent PlayerInitialized => s_playerInitializedEvent.Value;

    private static readonly Lazy<JoinEvent> s_playerJoinEvent = new(() => new());
    public static JoinEvent PlayerJoin => s_playerJoinEvent.Value;
    private static readonly Lazy<LeftEvent> s_playerLeftEvent = new(() => new());
    public static LeftEvent PlayerLeft => s_playerLeftEvent.Value;
    private static readonly Lazy<RespawnEvent> s_playerRespawnEvent = new(() => new());
    public static RespawnEvent PlayerRespawn => s_playerRespawnEvent.Value;
    private static readonly Lazy<DeathEvent> s_playerDeathEvent = new(() => new());
    public static DeathEvent PlayerDeath => s_playerDeathEvent.Value;
}
