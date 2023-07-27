using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class ScoreboardHook : HookBase<ScoreboardHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(void* @this, void* a2, void* a3);

    public ScoreboardHook()
        : base("_ZN16ServerScoreboardC2E23CommandSoftEnumRegistryP12LevelStorage") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            Log.Logger.Trace(nameof(ScoreboardHook));
            var result = Original(@this, a2, a3);
            Global.Scoreboard.Instance = new Scoreboard(@this);
            TryUninstall();
            return result;
        };
}