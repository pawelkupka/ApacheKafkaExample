using System;
using System.Threading;
using Confluent.Kafka;

namespace ApacheKafkaExample.Producer1
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
                for (int i = 0; i < 10; ++i)
                {
                    p.Produce("my-topic1", new Message<Null, string> { Value = i.ToString() }, handler);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                p.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
