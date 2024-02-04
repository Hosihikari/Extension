namespace Hosihikari.Minecraft.Extension.GlobalService.Hook;

// internal sealed class AllowListFileHook : HookBase<AllowListFileHook.HookDelegate>
// {
//     internal unsafe delegate Pointer<AllowListFile> HookDelegate(Pointer<AllowListFile> @this, void* a2);
//
//     public AllowListFileHook()
//         : base(AllowListFile.Original.Constructor_AllowListFile) { }
//
//     public override unsafe HookDelegate HookedFunc =>
//         (@this, a2) =>
//         {
//             Log.Logger.LogTrace("In {ModuleName}", nameof(AllowListFileHook));
//             Pointer<AllowListFile> result = Original(@this, a2);
//             Global.AllowListFile.Instance = new(@this);
//             TryUninstall();
//             return result;
//         };
// }