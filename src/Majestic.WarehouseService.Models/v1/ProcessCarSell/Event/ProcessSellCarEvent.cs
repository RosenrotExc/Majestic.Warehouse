namespace Majestic.WarehouseService.Models.v1.ProcessCarSell.Event
{
    public class ProcessSellCarEvent
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string NewOwnerName { get; set; }
    }
}
