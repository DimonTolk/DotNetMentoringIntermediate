using Confluent.Kafka;
using MainProcessingService.Interfaces;
using MainProcessingService.Models;

namespace MainProcessingService
{
    public class MessageBus
    {
        private const string GroupId = "TestGroup";
        private readonly ConsumerConfig _consumerConfig;

        public MessageBus(string host)
        {
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = host,
                GroupId = GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public void SubscribeOnTopic(string topic)
        {
            using var consumer = new ConsumerBuilder<Ignore, DCSMessage>(_consumerConfig)
                                        .SetValueDeserializer(new CustomValueDeserializer<DCSMessage>())
                                        .Build();
            consumer.Subscribe(topic);

            var tokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (_, evnt) =>
            {
                evnt.Cancel = true;
                tokenSource.Cancel();
            };
            try
            {
                while (true)
                {
                    try
                    {
                        var consumerResult = consumer.Consume(tokenSource.Token);
                        if (consumerResult.Message.Value is DCSMessage result)
                        {
                            var processor = FileProcessorResolver.ResolveProcessor(result.FileName);
                            processor.Process(result);
                        }
                    }
                    catch (ConsumeException exception)
                    {
                        Console.WriteLine($"Error occurred: {exception.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}
