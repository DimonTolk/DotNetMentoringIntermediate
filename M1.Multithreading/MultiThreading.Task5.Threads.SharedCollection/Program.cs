/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private const int CollectionSize = 10;
        private static readonly Mutex _mutex = new Mutex();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var sharedCollection = new List<int>();
            var finished = false;

            var fillTask = new Task(() =>
            {
                for (int i = 0; i < CollectionSize; i++)
                {
                    _mutex.WaitOne();
                    sharedCollection.Add(i);
                    _mutex.ReleaseMutex();
                }

                finished = true;
            });

            var printTask = new Task(() =>
            {
                while (!finished)
                {
                    _mutex.WaitOne();
                    Console.WriteLine($"[{string.Join(",", sharedCollection)}]");
                    _mutex.ReleaseMutex();
                }
            });

            fillTask.Start();
            printTask.Start();
            Task.WaitAll(fillTask, printTask);

            Console.ReadLine();
        }
    }
}
