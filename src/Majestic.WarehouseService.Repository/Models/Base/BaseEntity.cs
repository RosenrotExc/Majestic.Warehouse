using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Repository.Models.Base
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public int CodeId { get; set; }
        public CarEntityCode Code { get; set; }
        public IList<CarEntityState> States { get; set; }
    }
}
