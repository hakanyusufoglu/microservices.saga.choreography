using Shared.Messages;

namespace Shared.Events
{
    public class StockReservedEvent
    {
        //Kimden hangi siparişten ödeme alınacak? bu entityler ile belirlenir.
        public Guid BuyerId { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
