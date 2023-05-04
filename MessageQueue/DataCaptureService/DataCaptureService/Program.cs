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
        var messageBus = new MessageBus(KafkaConfig.Host);
        try
        {
            if (content.Length >= Constants.KafkaMessageMaxSize)
            {
                foreach (var item in CreateMessageSequence(name, content))
                {
                    await messageBus.SendMessage(KafkaConfig.Topic, item);
                }
            }
            else
            {
                var message = new DCSMessage() { Position = 1, Size = 1, Content = content, FileName = name };
                await messageBus.SendMessage(KafkaConfig.Topic, message);
            }

            Console.WriteLine($"Created: {args.FullPath}");
            Console.WriteLine("Message sent");
        }
        catch
        {
            Console.WriteLine($"Error during message sending");
        }
    }

    private static IEnumerable<DCSMessage> CreateMessageSequence(string name, byte[] content)
    {
        var aproxSize = content.Length / Constants.KafkaMessageMaxSize;
        var size = content.Length % Constants.KafkaMessageMaxSize == 0 
                    ? aproxSize 
                    : aproxSize + 1;

        var contentToSend = content.Chunk(size).ToArray();
        var iterator = 0;
        foreach (var item in contentToSend)
        {
            yield return new DCSMessage() 
            { 
                FileName = name, 
                Position = iterator, 
                Size = size, 
                Content = contentToSend[iterator++]
            };
        }
    }
}