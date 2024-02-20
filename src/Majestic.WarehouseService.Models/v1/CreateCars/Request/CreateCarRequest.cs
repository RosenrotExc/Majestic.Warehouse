namespace Majestic.WarehouseService.Models.v1.CreateCars.Request
{
    public class CreateCarRequest
    {
        public string CarName { get; set; }
        public string ModelName { get; set; }
        public string OwnerName { get; set; }
        public decimal OwnersPrice { get; set; }
        public decimal DealersPrice { get; set; }
        public string DealerNotes { get; set; }
    }
}
