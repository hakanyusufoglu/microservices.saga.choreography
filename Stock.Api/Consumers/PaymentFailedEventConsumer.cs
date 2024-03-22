using MassTransit;
using Shared.Events;
using Stock.Api.Services;
using MongoDB.Driver;
namespace Stock.Api.Consumers
{
    public class PaymentFailedEventConsumer(MongoDbService mongoDbService) : IConsumer<PaymentFailedEvent>
    {
        // İptal edilen siparişe göre yapılan işlemleri geri alınmasını sağlar
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var stocks = mongoDbService.GetCollection<Models.Stock>();
            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await (await stocks.FindAsync(s => s.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();
                if (stock != null)
                {
                    stock.Count += orderItem.Count;
                    await stocks.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId.ToString(), stock);
                }
            }
        }
    }
}
