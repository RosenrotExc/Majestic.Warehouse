using Majestic.WarehouseService.Repository.Models.Cars;

namespace Majestic.WarehouseService.Repository.Models.Base
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        int CodeId { get; set; }
        CarEntityCode Code { get; set; }
        IList<CarEntityState> States { get; set; }
    }
}
