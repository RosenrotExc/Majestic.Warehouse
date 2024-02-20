namespace Majestic.WarehouseService.Repository.Models.Base
{
    public interface IBaseEntity<TBaseEntityState, TBaseEntityCode> 
        where TBaseEntityState : BaseEntityState
        where TBaseEntityCode : BaseEntityCode
    {
        int Id { get; set; }
        int CodeId { get; set; }
        TBaseEntityCode Code { get; set; }
        IList<TBaseEntityState> States { get; set; }
    }
}
