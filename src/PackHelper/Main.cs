using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Hosihikari.Minecraft.Extension.PackHelper;

public record PackInfo(string PackId, (int, int, int) Version, string? SubPack = null);

public static class Main
{
    private static List<PackInfo>? ResourcePacks = new();
    private static List<PackInfo>? BehaviorPacks = new();

    public static void AddResourcePack(PackInfo packInfo)
    {
        if (ResourcePacks is null)
            throw new NullReferenceException(nameof(ResourcePacks));
        ResourcePacks.Add(packInfo);
    }

    public static void AddBehaviorPack(PackInfo packInfo)
    {
        if (BehaviorPacks is null)
            throw new NullReferenceException(nameof(BehaviorPacks));
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
        // add customize pack
        if (ResourcePacks is not null) //first call from resource pack
        {
            foreach (var (id, (a, b, c), subPack) in ResourcePacks)
            {
                var pack = new JsonObject
                {
                    ["pack_id"] = id,
                    ["version"] = new JsonArray { a, b, c }
                };
                if (subPack is not null)
                    pack.Add("subpack", subPack);
                array.Add(pack);
            }

            ResourcePacks = null;
        }
        else if (BehaviorPacks is not null) //second call
        {
            foreach (var (id, (a, b, c), subPack) in BehaviorPacks)
            {
                var pack = new JsonObject
                {
                    ["pack_id"] = id,
                    ["version"] = new JsonArray { a, b, c }
                };
                if (subPack is not null)
                    pack.Add("subpack", subPack);
                array.Add(pack);
            }
            BehaviorPacks = null;
            //no longer need, uninstall hook
            _hook.Uninstall();
        }
    }
}
