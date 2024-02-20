using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCars.Response;
using Majestic.WarehouseService.Repository.Contexts;
using Majestic.WarehouseService.Repository.Models.Cars;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Repository.Repository.Cars
{
    public class CarsRepository : ICarsRepository
    {
        private readonly ILogger<CarsRepository> _logger;
        private readonly WarehouseDbContext _dbContext;

        public CarsRepository(
            ILogger<CarsRepository> logger,
            WarehouseDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> CreateCarAsync(CarEntity entity)
        {
            try
            {


                await _dbContext.AddAsync(entity);
                var affectedCount = await _dbContext.SaveChangesAsync();
                if (affectedCount < 0)
                {
                    const string Message = "Failed to create car";
                    _logger.LogError("{Message} {entity}", Message, entity);
                    return new ServiceResult(Message);
                }

                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to create car";
                _logger.LogError(ex, "{Message} {entity}", Message, entity);
                return new ServiceResult(Message);
            }
        }

        public async Task<PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter>> QueryCarsAsync(GetCarFilter filter)
        {
            try
            {
                var result = await _dbContext.Set<CarEntity>().ToListAsync();

                return null;
            }
            catch (Exception ex)
            {
                const string Message = "Failed to create car";
                _logger.LogError(ex, "{Message} {filter}", Message, filter);
                return new PaginatedServiceResultWrapper<GetCarResponse, GetCarFilter>
                {
                    Message = Message
                };
            }
        }
    }
}
