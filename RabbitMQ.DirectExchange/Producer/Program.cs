using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(CreateTask(12000, "error"));
            Task.Run(CreateTask(10000, "info"));
            Task.Run(CreateTask(8000, "warning"));

            Console.ReadKey();
        }

        static Func<Task> CreateTask(int timeToSleepTo, string routingKey)
        {
            return () =>
            {
                var counter = 0;
                do
                {
                    int timeToSleep = new Random().Next(1000, timeToSleepTo);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                        string message = $"Message type [{routingKey}] from publisher N {counter}";

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "direct_logs",
                                            routingKey: routingKey,
                                            basicProperties: null,
                                            body: body);

                        Console.WriteLine($"Message type [{routingKey}] is sent into Direct Exchange [N:{counter++}]");
                    }
                } while (true);
            };
        }
    }
}
