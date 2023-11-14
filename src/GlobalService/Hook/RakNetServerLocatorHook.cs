using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged.STL;
using Hosihikari.NativeInterop.Unmanaged;
using System.Runtime.InteropServices;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class RakNetServerLocatorHook : HookBase<RakNetServerLocatorHook.HookDelegate>
{
    internal unsafe delegate int HookDelegate(Pointer<RakNetServerLocator> @this, Reference<StdString> a1, Reference<StdString> a2, nint a3, int a4, int a5, int a6, bool a7, bool a8);

    public RakNetServerLocatorHook()
        : base(RakNetServerLocator.Original.StartAnnouncingServer)
    { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a1, a2, a3, a4, a5, a6, a7, a8) =>
        {
            var result = Original(@this, a1, a2, a3, a4, a5, a6, a7, a8);
            Global.RakNetServerLocator.Instance = new RakNetServerLocator(@this);
            TryUninstall();
            return result;
        };
}