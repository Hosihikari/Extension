using System.Text.Json.Nodes;

namespace Hosihikari.Minecraft.Extension.PackHelper;

public record PackInfo(Guid PackId, (int, int, int) Version, string? SubPack = null);

public class PackAlreadyLoadedException : Exception
{
    public PackAlreadyLoadedException()
        : base("Pack already loaded. Please add pack before server started.") { }
}

public class PackAlreadyAddedException : Exception
{
    public PackAlreadyAddedException(Guid packId)
        : base($"Pack {packId} already added.") { }
}

public static partial class PackHelper
{
    private static List<PackInfo>? ResourcePacks = new();
    private static List<PackInfo>? BehaviorPacks = new();

    public static void AddResourcePack(PackInfo packInfo)
    {
        if (ResourcePacks is null)
            throw new PackAlreadyLoadedException();
        if (ResourcePacks.Any(x => x.PackId == packInfo.PackId))
            throw new PackAlreadyAddedException(packInfo.PackId);
        ResourcePacks.Add(packInfo);
    }

    public static void AddBehaviorPack(PackInfo packInfo)
    {
        if (BehaviorPacks is null)
            throw new PackAlreadyLoadedException();
        if (BehaviorPacks.Any(x => x.PackId == packInfo.PackId))
            throw new PackAlreadyAddedException(packInfo.PackId);
        BehaviorPacks.Add(packInfo);
    }

    private static readonly PackStackHook _hook = new(ProcessWorldPacksJson);

    public static void Init()
    {
        //ResourcePackStack::deserialize
        _hook.Install();
    }

    /*world_resource_packs.json & world_behavior_packs.json
     {
        "pack_id" : "7d5214c2-6ca0-4aaf-8cd5-448a955f06ed",
        "version" : [ 1, 3, 0 ],
        "subpack" : "xxx"
    }
     */
    public static void ProcessWorldPacksJson(JsonArray array)
    {
        void Process(List<PackInfo> target)
        {
            foreach (var (id, (a, b, c), subPack) in target)
            {
                var pack = new JsonObject
                {
                    ["pack_id"] = id.ToString(),
                    ["version"] = new JsonArray { a, b, c }
                };
                if (subPack is not null)
                    pack.Add("subpack", subPack);
                array.Add(pack);
            }
        }

        // add customize pack
        if (ResourcePacks is not null) //first call from resource pack
        {
            Process(ResourcePacks);
            ResourcePacks = null;
        }
        else if (BehaviorPacks is not null) //second call
        {
            Process(BehaviorPacks);
            BehaviorPacks = null;
            //no longer need, uninstall hook
            _hook.Uninstall();
        }
    }
}
