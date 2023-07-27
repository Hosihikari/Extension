using System.Text;
using System.Text.Json.Nodes;
using Hosihikari.Minecraft.Extension.Events.Implements;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.NativeTypes;
using Hosihikari.NativeInterop.Utils;

namespace Hosihikari.Minecraft.Extension.PackHelper;

internal class PackStackHook : HookBase<PackStackHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(void* @this, void* a2, void* a3);

    public PackStackHook(Action<JsonArray> proceedFunc)
        : base(
            "_ZN17ResourcePackStack11deserializeERSiRKN3gsl8not_nullIN7Bedrock15NonOwnerPointerIK23IResourcePackRepositoryEEEE"
        )
    {
        _proceedFunc = proceedFunc;
    }

    private readonly Action<JsonArray> _proceedFunc;
    public override unsafe HookDelegate HookedFunc =>
        (resourcePackStack, stream, a3) =>
        {
            try
            {
                //load original steam (maybe a file stream from world_resource_packs.json)
                StdInputStream stdInputStream = new(stream);
                //convert to byte[] (utf8 string)
                var originalData = stdInputStream.ReadToEnd();
                var array = new JsonArray();
                if (originalData.Length > 2)
                {
                    try
                    { //parse world_resource_packs.json
                        array = JsonNode.Parse(originalData)?.AsArray()!;
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error("Error Parsing PackStack", e);
                    }
                }
                //add custom pack
                {
                    _proceedFunc(array);
                }
                //add fake stream from modified world_resource_packs.json and pass to original function
                StdInputStream fakeStream = new(StringUtils.StringToManagedUtf8(array.ToString()));
                GC.KeepAlive(fakeStream);
                return Original(resourcePackStack, fakeStream, a3);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(nameof(PackStackHook), ex);
                return Original(resourcePackStack, stream, a3);
            }
        };
}
