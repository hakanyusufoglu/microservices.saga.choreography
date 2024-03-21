﻿using Order.Api.Enums;

namespace Order.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public OrderStatus orderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public Decimal TotalPice { get; set; }
    }
}
