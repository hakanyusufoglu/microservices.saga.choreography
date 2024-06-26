﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models.Contexts;
using Shared.Events;

namespace Order.Api.Consumers
{
    //Normalde bir servis ile iletişim kurulur ve ödeme başarısız olduğunda siparişin iptal edilmesi sağlanır
    public class PaymentFailedEventConsumer(OrderApiDbContext _context) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
                throw new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Fail;
            await _context.SaveChangesAsync();
        }
    }
}
