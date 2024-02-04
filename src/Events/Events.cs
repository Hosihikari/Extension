using Hosihikari.Minecraft.Extension.Events.Implements.Player;

namespace Hosihikari.Minecraft.Extension.Events;

public static class Events
{
    private static readonly Lazy<ChatEvent> s_playerChatEvent = new(() => new());

    private static readonly Lazy<InitializedEvent> s_playerInitializedEvent = new(() => new());

    private static readonly Lazy<JoinEvent> s_playerJoinEvent = new(() => new());
    private static readonly Lazy<LeftEvent> s_playerLeftEvent = new(() => new());
    private static readonly Lazy<RespawnEvent> s_playerRespawnEvent = new(() => new());
    private static readonly Lazy<DeathEvent> s_playerDeathEvent = new(() => new());
    public static ChatEvent PlayerChat => s_playerChatEvent.Value;
    public static InitializedEvent PlayerInitialized => s_playerInitializedEvent.Value;
    public static JoinEvent PlayerJoin => s_playerJoinEvent.Value;
    public static LeftEvent PlayerLeft => s_playerLeftEvent.Value;
    public static RespawnEvent PlayerRespawn => s_playerRespawnEvent.Value;
    public static DeathEvent PlayerDeath => s_playerDeathEvent.Value;
}