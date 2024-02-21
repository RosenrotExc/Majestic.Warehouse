using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarQuery.QueryModels
{
    public class GetCarsModelQuery
    {
        public GetCarFilter Filter { get; }
        public Initiator Initiator { get; }

        public GetCarsModelQuery(GetCarFilter filter, Initiator initiator)
        {
            Filter = filter;
            Initiator = initiator;
        }
    }
}
