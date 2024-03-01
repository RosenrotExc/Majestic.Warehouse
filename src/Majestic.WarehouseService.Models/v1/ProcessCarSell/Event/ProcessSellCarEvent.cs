using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Models.v1.ProcessCarSell.Event
{
    public class ProcessSellCarEvent : BasicEvent
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string NewOwnerName { get; set; }
    }
}
