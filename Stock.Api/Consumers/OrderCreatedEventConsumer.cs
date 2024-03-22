using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Stock.Api.Services;

namespace Stock.Api.Consumers
{
    public class OrderCreatedEventConsumer(MongoDbService mongoDbService) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            //kullanıcıdan gelen siparişlerden hangileri stokta var kontrolü sağlanır.
            List<bool> stockResult = new();
            IMongoCollection<Models.Stock> collection = mongoDbService.GetCollection<Models.Stock>();

            //OrderItems'lara karşılık stokta yeterli ürün var mı 

            foreach (var orderItem in context.Message.OrderItems)
            {
                stockResult.Add(await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).AnyAsync());
            }

            //Eğer tüm ürünler stokta var mı
            if (stockResult.TrueForAll(s => s.Equals(true)))
            {
                //stock güncellemesi yapılır.
                foreach (var orderItem in context.Message.OrderItems)
                {
                    Models.Stock stock = await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

                    stock.Count -=orderItem.Count;
                    
                    // FindOneAndReplaceAsync metodu ile stock güncellenir.
                    await collection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
                }


                //payment servisi uyaracak event'in fırlatılması
            }
            else
            {
                //stok işlemi başarısız
                //order'ı uyaracak event'in fırlatılması
            }

        }
    }
}
