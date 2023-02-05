using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer.All.Usd
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "Consumer: Counts all USD transfers";

            int counter = 0;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("x-match", "any"); //we can ignore that header if it's "any"
                headers.Add("currency", "usd");

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                         exchange: "headers_ex",
                                         routingKey: "-",
                                         arguments: headers);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());

                    counter++;

                    Console.WriteLine($"Received message: {message} {Environment.NewLine}" +
                        $"Total USD transfers: {counter}");
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine($"Subscribed to the queue '{queueName}'");
                Console.WriteLine($"Counts all USD transfers");

                Console.ReadLine();
            }
        }
    }
}