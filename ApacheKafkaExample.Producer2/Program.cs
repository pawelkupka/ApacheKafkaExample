using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace ApacheKafkaExample.Producer2
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            Action<DeliveryReport<Null, string>> handler = r =>
            Console.WriteLine(!r.Error.IsError
                ? $"Delivered message to {r.TopicPartitionOffset}"
                : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                for (int i = 0; i < 100; ++i)
                {
                    p.Produce("my-topic2", new Message<Null, string> { Value = i.ToString() }, handler);
                }

                p.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
