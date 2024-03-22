namespace Order.Api.ViewModels
{
    public class CreateOrderItemVm
    {
        //Guid tipindeki verileri request işleminde string olarak karşılıyoruz.
        public string ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
