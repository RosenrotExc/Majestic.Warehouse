using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics.QueryModels
{
    public class GetCarsMetricsModelQuery
    {
        public Initiator Initiator { get; }

        public GetCarsMetricsModelQuery(Initiator initiator)
        {
            Initiator = initiator;
        }
    }
}
