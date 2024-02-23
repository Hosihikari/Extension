using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class ScoreboardHook()
    : HookBase<ScoreboardHook.HookDelegateType>(ServerScoreboard.Original.Constructor_ServerScoreboard)
{
    protected override unsafe HookDelegateType HookDelegate =>
        (@this, a2, a3) =>
        {
            void* result = Original(@this, a2, a3);
            Global.Scoreboard.Instance = @this.Target.As<Scoreboard>();
            TryUninstall();
            return result;
        };

    internal unsafe delegate void* HookDelegateType(Pointer<ServerScoreboard> @this, Pointer<CommandRegistry> a2,
        Pointer<LevelStorage> a3);
}