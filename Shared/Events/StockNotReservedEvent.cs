namespace Shared.Events
{
    // stokta problem olduğunda fırlatılacak event
    public class StockNotReservedEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public string Message { get; set; } 
    }
}
