using Confluent.Kafka;
using MainProcessingService.Models;

namespace MainProcessingService
{
    public class MessageBus
    {
        private const string GroupId = "TestGroup";
        private readonly ConsumerConfig _consumerConfig;
        private Dictionary<string, List<byte>> _messages;

        public MessageBus(string host)
        {
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = host,
                GroupId = GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true,
            };

            _messages = new Dictionary<string, List<byte>>();
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
                            if(result.Position != result.Size)
                            {
                                _messages[result.FileName].AddRange(result.Content);
                            }
                            else
                            {
                                var processor = FileProcessorResolver.ResolveProcessor(result.FileName);
                                var fileContent = _messages[result.FileName].ToArray();
                                processor.Process((result.FileName, fileContent));
                            }
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
