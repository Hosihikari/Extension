namespace Hosihikari.Minecraft.Extension.Async;

public class Core
{
    /// <summary>
    /// Queue in thread pool.
    /// </summary>
    public static void QueueWorkItem(Action act)
    {
        ThreadPool.QueueUserWorkItem(
            x =>
            {
                try
                {
                    x();
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
            },
            act,
            true
        );
    }
}
