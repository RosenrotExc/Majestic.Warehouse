using Majestic.WarehouseService.Repository.Models.Base;

namespace Majestic.WarehouseService.Repository.Models.Internal
{
    public class State : BaseEntityType
    {
        public enum Values : byte
        {
            Inserted = 1,
            Updated,
            Deleted
        }
    }
}
