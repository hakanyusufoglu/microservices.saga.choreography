﻿namespace Order.Api.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public Decimal Price { get; set; }
    }
}
