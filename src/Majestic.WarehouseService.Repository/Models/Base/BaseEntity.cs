namespace Majestic.WarehouseService.Repository.Models.Base
{
    public abstract class BaseEntity<TBaseEntityState, TBaseEntityCode> 
        : IBaseEntity<TBaseEntityState, TBaseEntityCode> 
            where TBaseEntityState : BaseEntityState
            where TBaseEntityCode : BaseEntityCode
    {
        public int Id { get; set; }
        public int CodeId { get; set; }
        public TBaseEntityCode Code { get; set; }
        public IList<TBaseEntityState> States { get; set; }
    }
}
