namespace Majestic.WarehouseService.Repository.Models.Base
{
    public abstract class BaseEntityCode : IBaseEntityCode
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
