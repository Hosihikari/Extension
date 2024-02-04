using Hosihikari.Minecraft.Extension.GlobalService.Hook;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public static class Global
{
    public static GlobalInstance<Core.FilePathManager> FilePathManager { get; } = new(() => new FilePathManagerHook());
    public static GlobalInstance<Minecraft> Minecraft { get; } = new(() => new MinecraftHook());

    public static GlobalInstance<ServerNetworkHandler> ServerNetworkHandler { get; } =
        new(() => new ServerNetworkHandlerHook());

    public static GlobalInstance<MinecraftCommands> MinecraftCommands { get; } =
        new(() => new MinecraftCommandsHook());

    public static GlobalInstance<DBStorage> DbStorage { get; } = new();
    public static GlobalInstance<LevelStorage> LevelStorage { get; } = new();

    public static GlobalInstance<RakNetServerLocator> RakNetServerLocator { get; } =
        new(() => new RakNetServerLocatorHook());

    public static GlobalInstance<RakNet.RakPeer> RakPeer { get; } =
        new(() => new RakPeerHook());

    // public static GlobalInstance<AllowListFile> AllowListFile { get; } =
    //     new(() => new AllowListFileHook());

    public static GlobalInstance<Scoreboard> Scoreboard { get; } =
        new(() => new ScoreboardHook());

    public static GlobalInstance<StructureManager> StructureManager { get; } =
        new(() => new StructureManagerHook());

    internal static void Init()
    {
        new DbStorageHook().Install();
    }
}