using Hosihikari.NativeInterop.Utils;

namespace Hosihikari.Minecraft.Extension.PackHelper;

public enum PackType
{
    ResourcePack,
    BehaviorPack,
    Unknown = -1 //todo support auto detect ?
}

public static partial class PackHelper
{
    public static void AddPack(PackType packType, string packDirectory, PackInfo info)
    {
        if (!Directory.Exists(packDirectory))
            throw new DirectoryNotFoundException($"packDirectory {packDirectory} not found");
        var target = Path.Combine(
            Environment.CurrentDirectory,
            packType switch
            {
                PackType.BehaviorPack => "development_behavior_packs",
                PackType.ResourcePack => "development_resource_packs",
                _ => throw new ArgumentOutOfRangeException(nameof(packType), packType, null)
            }
        );
        if (!Directory.Exists(target))
            Directory.CreateDirectory(target);
        var link = Path.Combine(target, info.PackId.ToString());
        if (Directory.Exists(link))
        {
            if (LinkUtils.IsLink(link))
            {
                var current = LinkUtils.ReadLink(link);
                if (current == packDirectory)
                {
                    LinkUtils.Unlink(link);
                }
            }
            else
            {
                Directory.Delete(link, true);
            }
        }
        if (!Directory.Exists(link))
            LinkUtils.CreateDirectorySymlink(link, packDirectory);
        if (packType is PackType.BehaviorPack)
            AddBehaviorPack(info);
        else
            AddResourcePack(info);
    }
}
