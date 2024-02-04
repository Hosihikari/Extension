using Hosihikari.NativeInterop.Utils;
using System.Runtime.InteropServices;

namespace Hosihikari.Minecraft.Extension.PackHelper;

#if !WINDOWS
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
        {
            throw new DirectoryNotFoundException($"packDirectory {packDirectory} not found");
        }

        string target = System.IO.Path.Combine(
            Environment.CurrentDirectory,
            packType switch
            {
                PackType.BehaviorPack => "development_behavior_packs",
                PackType.ResourcePack => "development_resource_packs",
                PackType.Unknown => throw new IndexOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException(nameof(packType), packType, null)
            }
        );
        if (!Directory.Exists(target))
        {
            Directory.CreateDirectory(target);
        }

        string link = System.IO.Path.Combine(target, info.PackId.ToString());
        if (Directory.Exists(link))
        {
            if (LinkUtils.IsLink(link))
            {
                string current = LinkUtils.ReadLink(link);
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
                        {
                            Directory.CreateDirectory(destFolder);
                        }

                        string[] files = Directory.GetFiles(sourceFolder);
                        foreach (string file in files)
                        {
                            string name = System.IO.Path.GetFileName(file);
                            string dest = System.IO.Path.Combine(destFolder, name);
                            File.Copy(file, dest);
                        }

                        string[] folders = Directory.GetDirectories(sourceFolder);
                        foreach (string folder in folders)
                        {
                            string name = System.IO.Path.GetFileName(folder);
                            string dest = System.IO.Path.Combine(destFolder, name);
                            CopyFolder(folder, dest);
                        }
                    }
                }
            }
        }

        if (packType is PackType.BehaviorPack)
        {
            AddBehaviorPack(info);
        }
        else
        {
            AddResourcePack(info);
        }
    }
}
#endif