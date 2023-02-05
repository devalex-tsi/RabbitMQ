using RabbitMQ.Client;
using System.Text;

namespace Producer
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "Produser";

            Thread.Sleep(2000);
            int count = 0;
            Task.Run(() =>
            {
                do
                {
                    int timeToSleep = new Random().Next(2000, 7000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);

                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add("currency", "usd");
                        headers.Add("transfer", "abroad");

                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        string message = $"[{count}] sent with headers: [currencu:usd] [transfer:abroad]";

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "headers_ex",
                                            routingKey: "doesnt metter",
                                            basicProperties: properties,
                                            body: body);

                        Console.WriteLine($"[{count++} USD transfer from abroad]");
                    }
                } while (true);
            });

            Task.Run(() =>
            {
                do
                {
                    int timeToSleep = new Random().Next(2000, 5000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);

                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add("currency", "usd");
                        headers.Add("transfer", "internal");

                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        string message = $"[{count}] sent with headers: [currencu:usd] [transfer:internal]";

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "headers_ex",
                                            routingKey: "doesnt metter",
                                            basicProperties: properties,
                                            body: body);

                        Console.WriteLine($"[{count++} USD transfer internal]");
                    }
                } while (true);
            });

            Task.Run(() =>
            {
                do
                {
                    int timeToSleep = new Random().Next(2000, 6000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);

                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add("currency", "eur");
                        headers.Add("transfer", "internal");

                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        string message = $"[{count}] sent with headers: [currencu:usd] [transfer:internal]";

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "headers_ex",
                                            routingKey: "doesnt metter",
                                            basicProperties: properties,
                                            body: body);

                        Console.WriteLine($"[{count++} EUR transfer internal]");
                    }
                } while (true);
            });

            Console.ReadKey();
        }
    }
}
