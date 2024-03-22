namespace Order.Api.ViewModels
{
    public class CreateOrderVm
    {
        public string BuyerId { get; set; }
        public List<CreateOrderItemVm> OrderItems { get; set; }
    }
}
