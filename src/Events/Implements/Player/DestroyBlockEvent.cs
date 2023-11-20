using Hosihikari.NativeInterop;
using Hosihikari.NativeInterop.Unmanaged;
using System.Runtime.InteropServices;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public class DestroyBlockEventArgs : CancelableEventArgsBase
{
    public required Hosihikari.Minecraft.Player Player { get; init; }
    public required BlockPos Position { get; init; }
    public int DimensionId { get; init; }
}

public class DestroyBlockEvent : HookEventBase<DestroyBlockEventArgs, DestroyBlockEvent.HookDelegate>
{
    [return: MarshalAs(UnmanagedType.U1)]
    public unsafe delegate bool HookDelegate(Pointer<Block> @this, Pointer<Hosihikari.Minecraft.Player> player, BlockPos* pos);

    public DestroyBlockEvent()
        : base(Block.Original.PlayerWillDestroy) { }

    private const int DimensionIdOffset = 352;

    public override unsafe HookDelegate HookedFunc =>
        (@this, _player, pos) =>
        {
            var player = _player.Target;
            using var actor = player.As<Actor>();
            using var dim = actor.Dimension.Target;
            var dimid = DAccess<int>(dim.Pointer, DimensionIdOffset);

            var e = new DestroyBlockEventArgs
            {
                Player = player,
                DimensionId = dimid,
                Position = *pos
            };

            OnEventBefore(e);

            if (e.IsCanceled)
                return false;

            var ret = Original(@this, _player, pos);

            OnEventAfter(e);
            return ret;
        };

}
