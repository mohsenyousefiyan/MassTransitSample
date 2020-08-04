using MassTransit;
using MassTransitSample.Contracts;
using System;
using System.Threading.Tasks;

namespace MassTransitSample.ConsoleConsumer
{
    public class OrderRegisteredHandler :IConsumer<OrderRegistered>
    {       
        public async Task Consume(ConsumeContext<OrderRegistered> context)
        {
            System.Threading.Thread.Sleep(5);
            if (context.Message.OrderId == 2)
            {
                await context.RespondAsync<OrderRejected>(new OrderRejected
                {
                    RejectBy = "Mohsen Yousefiyan Rejector",
                    Reason = "Nothing else matter",
                    RejectDate = DateTime.Now,
                    OrderId = context.Message.OrderId

                }); ;
            }
            else
            {
                await context.RespondAsync<OrderAccepted>(new OrderAccepted
                {
                    AcceptBy = "Mohsen Yousefiyan",
                    AcceptDate = DateTime.Now,
                    OrderId = context.Message.OrderId
                });
            }
        }
    }
}
