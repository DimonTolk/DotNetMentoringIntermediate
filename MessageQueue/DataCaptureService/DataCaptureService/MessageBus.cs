using Confluent.Kafka;
using DataCaptureService.Models;

namespace DataCaptureService
{
    public class MessageBus
    {
        private readonly ProducerConfig _producerConfig;


        public MessageBus(string host)
        {
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = host
            };
        }

        public async Task SendMessage(string topic, DCSMessage message)
        {
            using var producer = new ProducerBuilder<Null, DCSMessage>(_producerConfig)
                                .SetValueSerializer(new CustomValueSerializer<DCSMessage>())
                                .Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync(topic, new Message<Null, DCSMessage> { Value = message });
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> exception)
            {
                Console.WriteLine($"Delivery failed: {exception.Error.Reason}");
            }

        }
    }
}
