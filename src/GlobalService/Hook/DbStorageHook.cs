using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class DbStorageHook : HookBase<DbStorageHook.HookDelegate>
{
    internal unsafe delegate Pointer<DBStorage> HookDelegate(Pointer<DBStorage> @this, void* a2, void* a3);

    public DbStorageHook()
        : base(DBStorage.Original.Constructor_DBStorage) { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {

            Log.Logger.Trace(nameof(DbStorageHook));
            var result = Original(@this, a2, a3);
            Global.DbStorage.Instance = @this.Target;
            Global.LevelStorage.Instance = @this.Target.As<LevelStorage>();
            TryUninstall();
            return result;
        };
}
