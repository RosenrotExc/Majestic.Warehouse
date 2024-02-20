using Majestic.WarehouseService.Repository.Models.Base;

namespace Majestic.WarehouseService.Repository.Models.Cars
{
    public class CarEntity : BaseEntity<CarEntityState, CarEntityCode>
    {
        public string CarName { get; set; }
        public string ModelName { get; set; }
        public string OwnerName { get; set; }
        public decimal OwnersPrice { get; set; }
        public decimal DealersPrice { get; set; }
        public string DealerNotes { get; set; }
    }
}
