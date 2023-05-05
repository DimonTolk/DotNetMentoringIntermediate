using PasswordHash;
using System.Diagnostics;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var saltValue = "qwertyyiuoqweqwe";
        var salt = Encoding.UTF8.GetBytes(saltValue);
        var password = "qwerty12345Aasd!";

        Console.WriteLine("Old variation:");
        var oldPasswordGenerator = new PasswordGeneratorOld();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var saltedPassword = oldPasswordGenerator.GeneratePasswordHashUsingSalt(password, salt);
        stopwatch.Stop();
        Console.WriteLine($"Elapsed: {stopwatch.ElapsedTicks}; Result: {saltedPassword}");

        var stopwatchNew = new Stopwatch();
        Console.WriteLine("New variation:");
        var newPasswordGenerator = new PasswordGenerator();
        stopwatchNew.Start();
        var saltedPasswordNew = newPasswordGenerator.GeneratePasswordHashUsingSalt(password, salt);
        stopwatchNew.Stop();
        Console.WriteLine($"Elapsed: {stopwatchNew.ElapsedTicks}; Result: {saltedPasswordNew}");
    }
}