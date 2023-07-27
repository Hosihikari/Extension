 using Hosihikari.NativeInterop.Hook.ObjectOriented;


namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

internal class DbStorageHook : HookBase<DbStorageHook.HookDelegate>
{
    internal unsafe delegate void* HookDelegate(void* @this, void* a2, void* a3);

    public DbStorageHook()
        : base("_ZN9DBStorageC2E15DBStorageConfigN3gsl8not_nullIN7Bedrock15NonOwnerPointerI10LevelDbEnvEEEE") { }

    public override unsafe HookDelegate HookedFunc =>
        (@this, a2, a3) =>
        {
            Log.Logger.Trace(nameof(DbStorageHook));
            var result = Original(@this, a2, a3);
            Global.DbStorage.Instance = new DbStorage(@this);
            Global.LevelStorage.Instance = new LevelStorage(@this);
            TryUninstall();
            return result;
        };
}
