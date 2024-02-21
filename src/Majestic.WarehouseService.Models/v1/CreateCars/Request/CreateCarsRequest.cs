namespace Majestic.WarehouseService.Models.v1.CreateCars.Request
{
    public class CreateCarsRequest
    {
        public IEnumerable<CreateCarRequest> Requests { get; set; }
    }
}
