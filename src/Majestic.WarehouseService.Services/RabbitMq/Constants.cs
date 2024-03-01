namespace Majestic.WarehouseService.Services.RabbitMq
{
    public class Constants
    {
        #region ProcessSellCar
        public const string ProcessSellCarQueueName = "warehouse-process-sell-car-queue-v1";
        public const string ProcessSellCarExchangeName = "warehouse-process-sell-car-exchange-v1";
        public const string ProcessSellCarDeadLetterQueueName = "warehouse-process-sell-car-queue-v1.deadletter";
        public const string ProcessSellCarDeadLetterExchangeName = "warehouse-process-sell-car-exchange-v1.deadletter";
        #endregion

        #region System
        public const string SystemExchangeTypeXDelayedMessage = "x-delayed-message";
        #endregion
    }
}
