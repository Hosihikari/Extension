using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Hosihikari.NativeInterop.Unmanaged;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal sealed class DbStorageHook() : HookBase<DbStorageHook.HookDelegate>(DBStorage.Original.Constructor_DBStorage)
{
    internal unsafe delegate Pointer<DBStorage> HookDelegate(Pointer<DBStorage> @this, void* a2, void* a3);

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {

            Log.Logger.LogTrace("In {ModuleName}", nameof(DbStorageHook));
            Pointer<DBStorage> result = Original(@this, a2, a3);
            Global.DbStorage.Instance = @this.Target;
            Global.LevelStorage.Instance = @this.Target.As<LevelStorage>();
            TryUninstall();
            return result;
        };
}
