using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class ScoreboardHook()
    : HookBase<ScoreboardHook.HookDelegate>(ServerScoreboard.Original.Constructor_ServerScoreboard)
{
    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            void* result = Original(@this, a2, a3);
            Global.Scoreboard.Instance = @this.Target.As<Scoreboard>();
            TryUninstall();
            return result;
        };

    internal unsafe delegate void* HookDelegate(Pointer<ServerScoreboard> @this, Pointer<CommandRegistry> a2,
        Pointer<LevelStorage> a3);
}