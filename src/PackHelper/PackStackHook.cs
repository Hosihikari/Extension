using System.Text.Json.Nodes;
using Hosihikari.Minecraft.Extension.Events.Implements;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged.STL;
using Hosihikari.NativeInterop.Utils;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.PackHelper;

internal sealed class PackStackHook(Action<JsonArray> proceedFunc)
    : HookBase<PackStackHook.HookDelegate>(ResourcePackStack.Original.Deserialize)
{
    internal unsafe delegate void* HookDelegate(void* @this, void* a2, void* a3);

    public override unsafe HookDelegate HookedFunc =>
        (resourcePackStack, stream, a3) =>
        {
            try
            {
                //load original steam (maybe a file stream from world_resource_packs.json)
                StdInputStream stdInputStream = new(stream);
                //convert to byte[] (utf8 string)
                byte[] originalData = stdInputStream.ReadToEnd();
                JsonArray array = [];
                if (originalData.Length > 2)
                {
                    try
                    { //parse world_resource_packs.json
                        array = JsonNode.Parse(originalData)?.AsArray()!;
                    }
                    catch (Exception e)
                    {
                        Log.Logger.LogError("Error Parsing PackStack: {Exception}", e);
                    }
                }
                //add custom pack
                {
                    proceedFunc(array);
                }
                //add fake stream from modified world_resource_packs.json and pass to original function
                StdInputStream fakeStream = new(StringUtils.StringToManagedUtf8(array.ToString()));
                GC.KeepAlive(fakeStream);
                return Original(resourcePackStack, fakeStream, a3);
            }
            catch (Exception ex)
            {
                Log.Logger.LogError("Unhandled Exception in {ModuleName}: {Exception}", nameof(PackStackHook), ex);
                return Original(resourcePackStack, stream, a3);
            }
        };
}
