using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InternalContextModels = Majestic.WarehouseService.Repository.Models.Internal;

namespace Majestic.WarehouseService.Repository.Repository.Initiators
{
    public class InitiatorRepository : IInitiatorRepository
    {
        private readonly ILogger<InitiatorRepository> _logger;
        private readonly WarehouseDbContext _dbContext;

        public InitiatorRepository(
            ILogger<InitiatorRepository> logger,
            WarehouseDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ServiceResultWrapper<InternalContextModels.Initiator>> CreateOrGetInitiatorAsync(string subjectId)
        {
            try
            {
                var initiator = await _dbContext.Set<InternalContextModels.Initiator>()
                    .Where(x => x.Value == subjectId)
                    .FirstOrDefaultAsync();

                if (initiator != null)
                {
                    return new ServiceResultWrapper<InternalContextModels.Initiator>(initiator);
                }

                var createModel = new InternalContextModels.Initiator
                {
                    Value = subjectId
                };

                await _dbContext.AddAsync(createModel);
                var affectedCount = await _dbContext.SaveChangesAsync();
                if (affectedCount < 0)
                {
                    const string Message = "Failed to create initiator";
                    _logger.LogError("{Message} {entity}", Message, createModel);
                    return new ServiceResultWrapper<InternalContextModels.Initiator>(Message);
                }

                return new ServiceResultWrapper<InternalContextModels.Initiator>(createModel);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to create or get initiator";
                _logger.LogError(ex, "{Message} {filter}", Message, subjectId);
                return new ServiceResultWrapper<InternalContextModels.Initiator>(Message);
            }
        }
    }
}
