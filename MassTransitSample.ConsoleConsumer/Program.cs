using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransitSample.ConsoleConsumer
{
    class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://localhost");

                cfg.ReceiveEndpoint("event-listener", e =>
                {
                    e.Consumer<OrderRegisteredHandler>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }            
        }
    }
}
