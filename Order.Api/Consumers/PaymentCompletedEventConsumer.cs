using MassTransit;
using Order.Api.Models.Contexts;
using Shared.Events;

namespace Order.Api.Consumers
{
    public class PaymentCompletedEventConsumer(OrderApiDbContext _context) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
                throw new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Completed;
            await _context.SaveChangesAsync();
        }
    }
}
