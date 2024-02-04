using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Hosihikari.NativeInterop;

namespace Hosihikari.Minecraft.Extension;

public static class LevelTick
{
    private static bool s_isInit;
    private static unsafe delegate* unmanaged<void*, void> s_originalFunc = null;

    //[MethodImpl(MethodImplOptions.Synchronized)]
    internal static void InitHook()
    {
        if (s_isInit)
            return;
        unsafe
        {
            if (
                NativeInterop.Hook.Function.Hook(
                    SymbolHelper.DlsymPointer(SymbolHelper.QuerySymbol(GameSession.Original.Tick)),
                    (delegate* unmanaged<void*, void>)&LevelTickHook,
                    out void* original,
                    out _
                ) is 0
            )
            {
                s_originalFunc = (delegate* unmanaged<void*, void>)original;
                s_tickQueue.Enqueue(() =>
                {
                    // set tick thread's _isInTickThread to true
                    IsInTickThread = true;
                });
            }
        }
        s_isInit = true;
    }

    [UnmanagedCallersOnly]
    internal static unsafe void LevelTickHook(void* @this)
    {
        if (!s_tickQueue.IsEmpty)
        {
            while (s_tickQueue.TryDequeue(out Action? action))
            {
                action();
            }
        }
        s_originalFunc(@this);
    }

    // The tick queue.
    private static readonly ConcurrentQueue<Action> s_tickQueue = new();

    // Whether the current thread is the tick thread.
    // [ThreadStatic] and must be set to true in the tick thread.

    /// <summary>
    /// Whether the current thread is the tick thread (MC main thread).
    /// </summary>
    [field: ThreadStatic]
    public static bool IsInTickThread { get; private set; }

    public static void PostTick(Action action)
    {
        s_tickQueue.Enqueue(action);
    }

    public static void RunInTick(Action action)
    {
        if (IsInTickThread)
            action();
        else
            s_tickQueue.Enqueue(action);
    }
}
