using Hosihikari.NativeInterop.Unmanaged;
using System.Runtime.InteropServices;

namespace Hosihikari.Minecraft.Extension.Events.Implements.Player;

public sealed class DestroyBlockEventArgs : CancelableEventArgsBase
{
    internal DestroyBlockEventArgs(Hosihikari.Minecraft.Player player, BlockPos position, int dimensionId = -1)
    {
        Player = player;
        Position = position;
        DimensionId = dimensionId;
    }
    public Hosihikari.Minecraft.Player Player { get; }
    public BlockPos Position { get; }
    public int DimensionId { get; }
}

public sealed class DestroyBlockEvent()
    : HookEventBase<DestroyBlockEventArgs, DestroyBlockEvent.HookDelegate>(Block.Original.PlayerWillDestroy)
{
    [return: MarshalAs(UnmanagedType.U1)]
    public unsafe delegate bool HookDelegate(Pointer<Block> @this, Pointer<Hosihikari.Minecraft.Player> player, BlockPos* pos);

    private const int DimensionIdOffset = 352;

    public override unsafe HookDelegate HookedFunc =>
        (@this, _player, pos) =>
        {
            Hosihikari.Minecraft.Player player = _player.Target;
            using Actor actor = player.As<Actor>();
            using Dimension dim = actor.Dimension.Target;
            int dimensionId = Memory.DAccess<int>(dim.Pointer, DimensionIdOffset);

            DestroyBlockEventArgs e = new(player, *pos, dimensionId);

            OnEventBefore(e);

            if (e.IsCanceled)
                return false;

            bool ret = Original(@this, _player, pos);

            OnEventAfter(e);
            return ret;
        };

}
