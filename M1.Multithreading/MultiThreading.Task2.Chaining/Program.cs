/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private const int MaxArrayLength = 10;
        private const int MinRandomValue = 1;
        private const int MaxRandomValue = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var arrayCreationTask = Task.Factory.StartNew(() =>
            {
                var array = new int[MaxArrayLength];
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = GetRandomNumber();
                }

                PrintArray(array);
                return array;
            });

            var multiplierTask = Task.Factory.StartNew(() => 
            {
                var createdArray = arrayCreationTask.Result;

                var multiplier = GetRandomNumber();
                Console.WriteLine($"Multiplier: {multiplier}");

                var modifiedArray = createdArray.Select(value => value * multiplier).ToArray();
                PrintArray(modifiedArray);
                return modifiedArray;
            });

            var sortingTask = Task.Factory.StartNew(() => 
            {
                var arrayToSort = multiplierTask.Result;
                Array.Sort(arrayToSort);
                PrintArray(arrayToSort);
                return arrayToSort;
            });

            var averageCalculationTask = Task.Factory.StartNew(() =>
            {
                var average = sortingTask.Result.Average();
                Console.WriteLine($"Average: {average}");
            });

            Console.ReadLine();
        }

        private static int GetRandomNumber(int maxValue = MaxRandomValue)
        {
            var random = new Random();

            return random.Next(MinRandomValue, maxValue);
        }

        private static void PrintArray(int[] array)
        {
            Console.WriteLine(string.Join(", ", array));
        }
    }
}
