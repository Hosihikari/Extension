using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class ScoreboardHook : HookBase<ScoreboardHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(Pointer<ServerScoreboard> @this, Pointer<CommandRegistry> a2, Pointer<LevelStorage> a3);

    public ScoreboardHook()
        : base(ServerScoreboard.Original.Constructor_ServerScoreboard) { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            Log.Logger.Trace(nameof(ScoreboardHook));
            var result = Original(@this, a2, a3);
            Global.Scoreboard.Instance = @this.Target.As<Scoreboard>();
            TryUninstall();
            return result;
        };
}