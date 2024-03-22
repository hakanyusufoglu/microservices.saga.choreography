using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stock.Api.Services;

namespace Stock.Api.Consumers
{
    public class OrderCreatedEventConsumer(MongoDbService mongoDbService, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            //kullanıcıdan gelen siparişlerden hangileri stokta var kontrolü sağlanır.
            List<bool> stockResult = new();
            IMongoCollection<Models.Stock> collection = mongoDbService.GetCollection<Models.Stock>();

            //OrderItems'lara karşılık stokta yeterli ürün var mı 

            foreach (var orderItem in context.Message.OrderItems)
            {
                stockResult.Add(await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId.ToString() && s.Count >= (long)orderItem.Count)).AnyAsync());
            }

            //Eğer tüm ürünler stokta var mı
            if (stockResult.TrueForAll(s => s.Equals(true)))
            {
                //stock güncellemesi yapılır.
                foreach (var orderItem in context.Message.OrderItems)
                {
                    Models.Stock stock = await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();

                    stock.Count -= orderItem.Count;

                    // FindOneAndReplaceAsync metodu ile stock güncellenir.
                    await collection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId.ToString(), stock);
                }


                //payment servisi uyaracak event'in fırlatılması
                var sendEndPoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMqSettings.Payment_StockReservedEventQueue}"));

                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice,
                    OrderItems = context.Message.OrderItems
                };

                //Send diyince hedef olan kuyruğa event fırlatılır.
                await sendEndPoint.Send(stockReservedEvent);
            }
            else
            {
                //stok işlemi başarısız
                //order'ı uyaracak event'in fırlatılması
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Stock miktarı yetersiz."
                };

                await publishEndpoint.Publish(stockNotReservedEvent);
            }

        }
    }
}
