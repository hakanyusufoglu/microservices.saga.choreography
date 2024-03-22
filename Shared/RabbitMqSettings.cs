namespace Shared
{
    public static class RabbitMqSettings
    {
        // hangi-servis-kullanicak_Hangi-eventi-kullanacak
        public const string Stock_OrderCreatedEventQueue = "stock_order_created_event-queue";
        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-queue";
        public const string Order_PaymentCompletedEventQueue = "order-payment-completed-event-queue";
        public const string Order_PaymentFailedEventQueue = "order-payment-failed-event-queue";
        public const string Stock_PaymentFailedEventQueue = "stock-payment-failed-event-queue";
    }
}
