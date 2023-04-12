/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            TaskA();
            TaskB();
            TaskC();
            TaskD();

            Console.ReadLine();
        }

        public static void TaskA()
        {
            var task1 = Task.Run(() => Method());
            task1.ContinueWith(Continuation, TaskContinuationOptions.None);

            task1.Wait();

            var tcs = new TaskCompletionSource<object>();
            var task2 = tcs.Task;

            tcs.SetException(new Exception());
            task2.ContinueWith(Continuation, TaskContinuationOptions.None);
        }

        public static void TaskB()
        {
            var tcs = new TaskCompletionSource<object>();
            var task = tcs.Task;

            tcs.SetException(new Exception());
            task.ContinueWith(Continuation, TaskContinuationOptions.OnlyOnFaulted);

            tcs = new TaskCompletionSource<object>();
            task = tcs.Task;

            tcs.SetCanceled();
            task.ContinueWith(Continuation, TaskContinuationOptions.OnlyOnCanceled);
        }

        public static void TaskC()
        {
            var tcs = new TaskCompletionSource<object>();
            var task = tcs.Task;

            tcs.SetException(new Exception());

            task.ContinueWith((t) => {
                Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId}");
                Continuation(t);
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        public static void TaskD()
        {
            var tcs = new TaskCompletionSource<object>();
            var task = tcs.Task;

            tcs.SetCanceled();
            task.ContinueWith((t) =>
            {
                var thread = new Thread(() => Continuation(t));
                Console.WriteLine($"TaskId: {t.Id}");
                thread.Start();

            }, TaskContinuationOptions.OnlyOnCanceled);
        }

        private static void Method()
        {
            Console.WriteLine($"Task #{Task.CurrentId} did this method in thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine(new string('-', 80));
        }

        private static void Continuation(Task task)
        {
            Console.WriteLine($"Continuation from task id={task.Id}");
            Console.WriteLine($"Continuation from task id={task.Id} did this method in thread {Thread.CurrentThread.ManagedThreadId}");
            
            if (task.IsFaulted)
            {
                Console.WriteLine($"is faulted from taskId:{task.Id}");
            }

            if (task.Exception != null)
            {
                Console.WriteLine($"with exception from taskId:{task.Id}");
            }

            Console.WriteLine();
        }
    }
}
