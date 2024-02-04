using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Hosihikari.NativeInterop.Unmanaged.STL;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class RakNetServerLocatorHook()
    : HookBase<RakNetServerLocatorHook.HookDelegate>(RakNetServerLocator.Original.StartAnnouncingServer)
{
    public override HookDelegate HookedFunc =>
        (@this, a1, a2, a3, a4, a5, a6, a7, a8) =>
        {
            int result = Original(@this, a1, a2, a3, a4, a5, a6, a7, a8);
            Global.RakNetServerLocator.Instance = new(@this);
            TryUninstall();
            return result;
        };

    internal delegate int HookDelegate(Pointer<RakNetServerLocator> @this, StdString a1, StdString a2, nint a3, int a4,
        int a5, int a6, bool a7, bool a8);
}