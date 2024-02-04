namespace Hosihikari.Minecraft.Extension.Async;

public static class Core
{
    /// <summary>
    ///     Queue in thread pool.
    /// </summary>
    public static void QueueWorkItem(Action act)
    {
        Task.Run(() =>
        {
            try
            {
                act();
            }
            catch (Exception)
            {
                try
                {
                    //Console.WriteLineErr(nameof(Async), ex, methodName, file, line);
                }
                catch
                {
                    // ignored
                }
            }
        });
    }
}