using MassTransit;
using Shared.Events;

namespace Payment.Api.Consumers
{
    public class StockReservedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<StockReservedEvent>
    {
        //Ödeme ile ilgili işlemler burada gerçekleştirilir. örneğin banka işlemleri vs yapılır ancak bu bir örnek sınıftır.
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)
            {
                //ödeme başarılı
                PaymentCompletedEvent paymentCompletedEvent = new PaymentCompletedEvent
                {
                    OrderId = context.Message.OrderId
                };
                await publishEndpoint.Publish(paymentCompletedEvent);
                await Console.Out.WriteLineAsync("Ödeme başarılı...");
            }
            else
            {
                //ödeme başarısız 
                //Burada Cheropgraphy patterni kullanılır.
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Ödeme başarısız (ex. Yetersiz Bakiye)",
                    OrderItemMessages = context.Message.OrderItems
                };

                await publishEndpoint.Publish(paymentFailedEvent);
                await Console.Out.WriteLineAsync("Ödeme başarısız...");
            }
        }
    }
}
