namespace Majestic.WarehouseService.Models.v1.ProcessSellCar.Request
{
    public class ProcessSellCarRequest
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string NewOwnerName { get; set; }
    }
}
