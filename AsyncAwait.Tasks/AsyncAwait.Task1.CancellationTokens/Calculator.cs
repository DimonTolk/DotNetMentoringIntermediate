using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static Task<long> Calculate(int n, CancellationToken token)
    {
        return Task.Run(() =>
        {
            long sum = 0;

            for (var i = 1; i <= n; i++)
            {
                if (token.IsCancellationRequested)
                    return sum;

                sum += i;
                Thread.Sleep(10);
            }

            return sum;
        },
        token);
    }
}
