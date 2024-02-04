using System.Runtime.CompilerServices;
using Hosihikari.NativeInterop.Hook.ObjectOriented;
using Microsoft.Extensions.Logging;

namespace Hosihikari.Minecraft.Extension.GlobalService;

public sealed class GlobalInstance<T>
    where T : class
{
    public GlobalInstance() { }

    public GlobalInstance(Func<IHook> hook)
    {
        hook().Install();
    }

    private T? _instance;
    public bool IsInitialized => _instance is not null;

    public T Instance
    {
        get
        {
            if (_instance is null)
            {
                throw new InvalidOperationException("Instance is not initialized.");
            }

            return _instance;
        }
        internal set
        {
            lock (this)
            {
                _instance = value;
                if (_onInitQueue is null)
                {
                    return;
                }

                //first init
                //call all callback and clear
                foreach (Action<T> action in _onInitQueue!)
                {
                    action(_instance);
                }

                _onInitQueue.Clear();
                _onInitQueue = null;
            }
        }
    }

    private Queue<Action<T>>? _onInitQueue = new();

    private void PostOnInit(Action<T> callback)
    {
        lock (this)
        {
            if (_onInitQueue is null)
            {
                //already init, call instantly
                callback(_instance!);
            }
            else
            {
                //not init, add to queue
                _onInitQueue.Enqueue(callback);
            }
        }
    }

    public void OnInit(
        Action<T> callback,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0
    )
    {
        PostOnInit(v =>
        {
            try
            {
                callback(v);
            }
            catch (Exception ex)
            {
                Log.Logger.LogError("Unhandled Exception in {ModuleName}: {Exception}\n  in {FileName}:{LineNumber}", GetType().Name + "::" + nameof(OnInit), ex, file, line);
            }
        });
    }

    public static implicit operator T(GlobalInstance<T> instance) => instance.Instance;
}