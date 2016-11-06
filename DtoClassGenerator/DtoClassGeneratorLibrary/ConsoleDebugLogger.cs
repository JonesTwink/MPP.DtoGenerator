using System;
using System.Threading;


namespace DtoClassGeneratorLibrary
{
    internal class ConsoleDebugLogger: IDebugLogger
    {
        public void PrintDebugInfo(bool isThreadPool = false)
        {
            
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (!isThreadPool)
            {
                int nWorkerThreads;
                int nCompletionThreads;
                ThreadPool.GetMaxThreads(out nWorkerThreads, out nCompletionThreads);
                Console.WriteLine("Total amount of threads available: {0}\nThe amount of IO-threads available: {1}", nWorkerThreads, nCompletionThreads);

                Console.WriteLine("Main thread. Is pool thread: {0}, Thread #: {1}", Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.GetHashCode());
            }
            else
            {
                Console.WriteLine("Worker thread. Is pool thread: {0}, Thread #: {1}", Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.GetHashCode());
            }
            Console.ResetColor();
        }

        
    }
}
