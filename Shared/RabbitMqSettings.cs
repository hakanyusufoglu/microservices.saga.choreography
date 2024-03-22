﻿namespace Shared
{
    public static class RabbitMqSettings
    {
        // hangi-servis-kullanicak_Hangi-eventi-kullanacak
        public const string Stock_OrderCreatedEventQueue = "stock_order_created_event-queue";
        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-queue";
    }
}
