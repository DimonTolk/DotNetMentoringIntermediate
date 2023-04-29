using DataCaptureService;
using DataCaptureService.Models;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("DCS started");
        using var watcher = new FileSystemWatcher(Constants.BucketPath);

        watcher.Created += OnCreated;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }

    private static async void OnCreated(object sender, FileSystemEventArgs args)
    {
        var file = File.ReadAllBytes(args.FullPath);
        var content = file;
        var name = args.Name;
        var message = new DCSMessage() { Content = content, FileName = name };
        Console.WriteLine($"Created: {args.FullPath}");

        var messageBus = new MessageBus(KafkaConfig.Host);
        await messageBus.SendMessage(KafkaConfig.Topic, message);
        Console.WriteLine("Message sent");
    }
}