namespace Majestic.WarehouseService.Repository.Models.Base
{
    public abstract class BaseEntityType : IBaseEntityType
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
