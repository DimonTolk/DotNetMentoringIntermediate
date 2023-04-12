/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private const int RecursionDepth = 10;
        private static readonly Semaphore _semaphore = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            ThreadPoolSemaphoreRecursive(RecursionDepth);

            var thread = RecursiveThreads();
            thread.Start(RecursionDepth);

            Console.ReadLine();
        }

        public static void ThreadPoolSemaphoreRecursive(int depth)
        {
            ThreadPool.QueueUserWorkItem(currentNumberObject =>
            {
                var decrementedNumber = DecrementNumber(currentNumberObject, nameof(ThreadPoolSemaphoreRecursive));

                if (decrementedNumber <= 0)
                {
                    _semaphore.Release();
                    return;
                }

                ThreadPoolSemaphoreRecursive(decrementedNumber);
            }, depth);

            _semaphore.WaitOne();
        }

        public static Thread RecursiveThreads()
        {
            return new Thread(currentNumberObject =>
            {
                var decrementedNumber = DecrementNumber(currentNumberObject, nameof(RecursiveThreads));

                if (decrementedNumber <= 0)
                {
                    return;
                }

                var newThread = RecursiveThreads();

                newThread.Start(decrementedNumber);
                newThread.Join();
            });
        }

        private static int DecrementNumber(object threadsState, string source)
        {
            var convertedThreadsState = Convert.ToInt32(threadsState);
            var decrementedNumber = --convertedThreadsState;

            Console.WriteLine($"Decremented number({source}) = '{decrementedNumber}'");

            return decrementedNumber;
        }
    }
}
