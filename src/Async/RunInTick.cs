using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Hosihikari.Minecraft.Extension.Async;

public sealed class RunInTickVoid : INotifyCompletion
{
    public static RunInTickVoid StartAsync(Action func)
    {
        RunInTickVoid asyncOperation = new(out Action reportResult, out Action<Exception> reportException);
        LevelTick.RunInTick(() =>
        {
            try
            {
                reportResult();
            }
            catch (Exception ex)
            {
                reportException(ex);
            }
        });
        return asyncOperation;
    }

    private RunInTickVoid(out Action reportResult, out Action<Exception> reportException)
    {
        reportResult = ReportResult;
        reportException = ReportException;
    }

    /// <summary>
    /// Get an awaitable object that can be used to await the await keyword asynchronously.
    /// This method will be called automatically by the compiler.
    /// </summary>
    /// <returns>
    /// Return itself for asynchronous waiting for return values.
    /// </returns>
    public RunInTickVoid GetAwaiter()
    {
        return this;
    }

    /// <summary>
    /// Get a state that indicates that the operation being asynchronously waited for has been completed (successfully or an exception occurred).
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Get the return value of this asynchronous waiting operation.
    /// This method will be called automatically by the compiler when await ends to get the return value.
    ///  Different from <see cref="Task{TResult}"/>, if the operation is not completed, this instance will return the default value of <typeparamref name="T"/> instead of blocking the thread until the task is completed. However, if an exception occurs in the asynchronous operation, calling this method will throw this exception.
    /// </summary>
    public void GetResult()
    {
        if (_exception is not null)
        {
            ExceptionDispatchInfo.Capture(_exception).Throw();
        }
    }

    /// <summary>
    /// When the method that executes the asynchronous task using this type is finished, the compiler will automatically call this method.
    /// That is to say, this method will be executed in the thread where the caller is located, which is used to notify the caller that the code in the thread where the caller is located has been executed and request to execute the subsequent task after await. In this type, the subsequent task is executed through <see cref="Core.QueueWorkItem(Action)"/>.
    /// </summary>
    /// <param name="continuation"> The subsequent task wrapped by the asynchronous task state machine. When executed, it will let the state machine continue to go down one step.</param>
    public void OnCompleted(Action continuation)
    {
        if (IsCompleted)
        {
            Core.QueueWorkItem(continuation);
        }
        else
        {
            _continuation += continuation;
        }
    }

    private void ReportResult()
    {
        IsCompleted = true;
        if (_continuation is not null)
        {
            Core.QueueWorkItem(_continuation);
        }
    }

    private void ReportException(Exception exception)
    {
        _exception = exception;
        if (_exception is not null)
        {
            //todo log
            //Console.WriteLineErr(nameof(RunInTickAsyncVoid), _exception );
        }
        IsCompleted = true;
        if (_continuation is not null)
        {
            Core.QueueWorkItem(_continuation);
        }
    }

    private Action? _continuation;

    private Exception? _exception;

    //public static RunInTickAsync<T> FromResult(T result)
    //{
    //    var asyncOperation = new RunInTickAsync<T>();
    //    asyncOperation.ReportResult(result);
    //    return asyncOperation;
    //}
    public void ContinueWith(Func<Task> action)
    {
        OnCompleted(() => action());
    }

    public void ContinueWith(Action action)
    {
        OnCompleted(action);
    }
}

public sealed class RunInTick<T> : INotifyCompletion //,IAwaitable<T>, IAwaiter<T>
{
    public static RunInTick<T> StartAsync(Func<T> func)
    {
        RunInTick<T> asyncOperation = new(out Action<T> reportResult, out Action<Exception> reportException);
        LevelTick.RunInTick(() =>
        {
            try
            {
                reportResult(func());
            }
            catch (Exception ex)
            {
                reportException(ex);
            }
        });
        return asyncOperation;
    }

    private RunInTick(
        out Action<T> reportResult,
        out Action<Exception> reportException
    )
    {
        reportResult = ReportResult;
        reportException = ReportException;
    }

    /// <summary>
    /// Get an awaitable object that can be used to await the await keyword asynchronously.
    /// </summary>
    /// <returns>
    /// Return itself for asynchronous waiting for return values.
    /// </returns>
    public RunInTick<T> GetAwaiter()
    {
        return this;
    }

    /// <summary>
    /// Get a state that indicates that the operation being asynchronously waited for has been completed (successfully or an exception occurred).
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Get the return value of this asynchronous waiting operation.
    /// Different from <see cref="Task{TResult}"/>, if the operation is not completed, this instance will return the default value of <typeparamref name="T"/> instead of blocking the thread until the task is completed. However, if an exception occurs in the asynchronous operation, calling this method will throw this exception.
    /// </summary>
    public T? Result
    {
        get => IsCompleted ? _result : default;
        private set => _result = value;
    }

    /// <summary>
    /// Manually get the return value of this asynchronous waiting operation asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<T> GetResultAsync()
    {
        await this;
        return _result!;
    }

    /// <summary>
    /// Get the return value of this asynchronous waiting operation.
    /// This method will be called automatically by the compiler when await ends to get the return value.
    /// </summary>
    public T GetResult()
    {
        if (_exception is not null)
        {
            // throw the exception that occurred in the asynchronous operation
            ExceptionDispatchInfo.Capture(_exception).Throw();
        }
        // return the result of the asynchronous operation
        return Result!;
    }

    /// <summary>
    /// When the method that executes the asynchronous task using this type is finished, the compiler will automatically call this method.
    /// That is to say, this method will be executed in the thread where the caller is located, which is used to notify the caller that the code in the thread where the caller is located has been executed and request to execute the subsequent task after await. In this type, the subsequent task is executed through <see cref="Core.QueueWorkItem(Action)"/>.
    /// </summary>
    /// <param name="continuation"> The subsequent task wrapped by the asynchronous task state machine. When executed, it will let the state machine continue to go down one step.</param>
    public void OnCompleted(Action continuation)
    {
        if (IsCompleted)
        {
            // if the task has been completed when the await starts, execute the code after the await directly.
            // Note that even if _continuation has a value, you don't need to care, because it will be executed when the report ends.
            Core.QueueWorkItem(continuation);
        }
        else
        {
            // When using multiple await keywords to wait for the same awaitable instance, this OnCompleted method will be executed multiple times.
            // When the task is really finished, all the code after these await needs to be executed.
            // So, we need to save all the code after await in _continuation, and execute them all when the task is finished.
            _continuation += continuation;
        }
    }

    private void ReportResult(T r)
    {
        Result = r;
        IsCompleted = true;
        if (_continuation is not null)
        {
            //Dispatcher.InvokeAsync(_continuation, _priority);
            Core.QueueWorkItem(_continuation);
        }
    }

    private void ReportException(Exception exception)
    {
        _exception = exception;
        if (_exception is not null)
        {
            //todo print exception?
            //   Console.WriteLineErr(nameof(RunInTickAsync<T>), _exception, methodName, file, line);
        }
        IsCompleted = true;
        if (_continuation is not null)
        {
            // todo ? Dispatcher.InvokeAsync(_continuation);
            // Queue the continuation action on the thread pool.
            Core.QueueWorkItem(_continuation);
        }
    }

    ///  <summary>
    /// action to save the continuation task after await temporarily, so that the task can continue to execute after the task is completed.
    /// </summary>
    private Action? _continuation;

    /// <summary>
    /// Temporarily save the exception that occurred during the execution of the asynchronous task. It will be thrown after the asynchronous waiting is over to report the error that occurred during the asynchronous execution.
    /// </summary>
    private Exception? _exception;
    private T? _result;

    //public static RunInTickAsync<T> FromResult(T result)
    //{
    //    var asyncOperation = new RunInTickAsync<T>();
    //    asyncOperation.ReportResult(result);
    //    return asyncOperation;
    //}
    public void ContinueWith(Func<T, Task> action)
    {
        OnCompleted(() => action(Result!));
    }

    public void ContinueWith(Action<T> action)
    {
        OnCompleted(() => action(Result!));
    }

    public static implicit operator T?(RunInTick<T> v)
    {
        return v.Result;
    }

    //public static RunInTickAsync<T> FromResult(T o)
    //{
    //    var asyncOperation = new RunInTickAsync<T>();
    //    asyncOperation.ReportResult(o);
    //    return asyncOperation;
    //}
}
