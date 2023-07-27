using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class RakNetServerLocatorHook : HookBase<RakNetServerLocatorHook.HookDelegate>
{
    internal unsafe delegate int HookDelegate(void* @this);

    public RakNetServerLocatorHook()
        : base(
            "_ZN19RakNetServerLocator9_activateEv") { }

    public override unsafe HookDelegate HookedFunc =>
        @this =>
        {
            var result = Original(@this);
            Global.RakNetServerLocator.Instance = new RakNetServerLocator(@this);
            TryUninstall();
            return result;
        };
}