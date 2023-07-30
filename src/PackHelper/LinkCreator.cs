using System.Runtime.InteropServices;
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
        {
            try
            {
                LinkUtils.CreateDirectorySymlink(link, packDirectory);
            }
            catch (ExternalException)
            {
                //file system not support symlink
                //just copy folder
                if (!Directory.Exists(link))
                {
                    CopyFolder(packDirectory, link);
                    void CopyFolder(string sourceFolder, string destFolder)
                    {
                        if (!Directory.Exists(destFolder))
                            Directory.CreateDirectory(destFolder);
                        var files = Directory.GetFiles(sourceFolder);
                        foreach (var file in files)
                        {
                            var name = Path.GetFileName(file);
                            var dest = Path.Combine(destFolder, name);
                            File.Copy(file, dest);
                        }
                        var folders = Directory.GetDirectories(sourceFolder);
                        foreach (var folder in folders)
                        {
                            var name = Path.GetFileName(folder);
                            var dest = Path.Combine(destFolder, name);
                            CopyFolder(folder, dest);
                        }
                    }
                }
            }
        }
        if (packType is PackType.BehaviorPack)
            AddBehaviorPack(info);
        else
            AddResourcePack(info);
    }
}
