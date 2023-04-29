using MainProcessingService;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("MPS started");
        var messageBus = new MessageBus(KafkaConfig.Host);
        messageBus.SubscribeOnTopic(KafkaConfig.Topic);
    }
}