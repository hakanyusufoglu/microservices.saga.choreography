using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models.Contexts;
using Shared.Events;

namespace Order.Api.Consumers
{
    public class StockNotReservedEventConsumer(OrderApiDbContext _context) : IConsumer<StockNotReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
                throw new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Fail;
            await _context.SaveChangesAsync();
        }
    }
}
