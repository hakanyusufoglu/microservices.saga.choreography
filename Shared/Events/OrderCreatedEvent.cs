using Shared.Messages;

namespace Shared.Events
{
    //Eventler geçmişe aittir.
    public class OrderCreatedEvent
    {
         //Buradaki veriler uyarıcağımız yani stock api'nin ihtiyayacı olacağı verileri eklemek mantıkılıdır.
         public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemMessage> OrderItems { get; set;}
    }
}
