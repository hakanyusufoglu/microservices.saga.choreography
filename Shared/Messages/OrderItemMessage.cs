namespace Shared.Messages
{
    //stock apinin ihtiyacı olacağı verileri barındıracak olan sınıf.
    public class OrderItemMessage
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
