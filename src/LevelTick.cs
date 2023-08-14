using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Hosihikari.NativeInterop;

namespace Hosihikari.Minecraft.Extension;

public static class LevelTick
{
    private static bool _isInit;
    private static unsafe delegate* unmanaged<void*, void> _originalFunc = null;

    //[MethodImpl(MethodImplOptions.Synchronized)]
    internal static void InitHook()
    {
        if (_isInit)
            return;
        unsafe
        {
            if (
                NativeInterop.Hook.NativeFunc.Hook(
                    SymbolHelper.DlsymPointer("_ZN11GameSession4tickEv"),
                    (delegate* unmanaged<void*, void>)&LevelTickHook,
                    out var original,
                    out _
                ) == 0
            )
            {
                _originalFunc = (delegate* unmanaged<void*, void>)original;
                _tickQueue.Enqueue(() =>
                {
                    // set tick thread's _isInTickThread to true
                    _isInTickThread = true;
                });
            }
        }
        _isInit = true;
    }

    [UnmanagedCallersOnly]
    internal static unsafe void LevelTickHook(void* @this)
    {
        if (!_tickQueue.IsEmpty)
        {
            while (_tickQueue.TryDequeue(out var action))
            {
                action();
            }
        }
        _originalFunc(@this);
    }

    // The tick queue.
    private static readonly ConcurrentQueue<Action> _tickQueue = new();

    // Whether the current thread is the tick thread.
    // [ThreadStatic] and must be set to true in the tick thread.
    [ThreadStatic]
    private static bool _isInTickThread;

    /// <summary>
    /// Whether the current thread is the tick thread (MC main thread).
    /// </summary>
    public static bool IsInTickThread => _isInTickThread;

    public static void PostTick(Action action)
    {
        _tickQueue.Enqueue(action);
    }

    public static void RunInTick(Action action)
    {
        if (_isInTickThread)
            action();
        else
            _tickQueue.Enqueue(action);
    }
}
