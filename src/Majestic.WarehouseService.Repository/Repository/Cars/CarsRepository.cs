using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
using Majestic.WarehouseService.Models.v1.GetCarsMetrics.Response;
using Majestic.WarehouseService.Repository.Contexts;
using Majestic.WarehouseService.Repository.Extensions;
using Majestic.WarehouseService.Repository.Models.Cars;
using Majestic.WarehouseService.Repository.Models.Internal;
using Majestic.WarehouseService.Repository.Repository.Initiators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InitiatorServiceModel = Majestic.WarehouseService.Models.Misc.Initiator;

namespace Majestic.WarehouseService.Repository.Repository.Cars
{
    public class CarsRepository : ICarsRepository
    {
        private readonly ILogger<CarsRepository> _logger;
        private readonly WarehouseDbContext _dbContext;
        private readonly IInitiatorRepository _initiatorRepository;

        public CarsRepository(
            ILogger<CarsRepository> logger,
            WarehouseDbContext dbContext,
            IInitiatorRepository initiatorRepository)
        {
            _logger = logger;
            _dbContext = dbContext;
            _initiatorRepository = initiatorRepository;
        }

        public async Task<ServiceResult> CreateCarAsync(List<CarEntity> entities, InitiatorServiceModel initiator)
        {
            try
            {
                var initiatorContext = await _initiatorRepository.CreateOrGetInitiatorAsync(initiator.SubjectId);

                foreach (var entity in entities)
                {
                    _logger.LogInformation("{name} {@entity}", nameof(CreateCarAsync), entity);

                    var utcNow = DateTime.UtcNow;

                    #region Create Code
                    entity.Code = new CarEntityCode
                    {
                        Value = Guid.NewGuid().ToString()
                    };
                    #endregion

                    #region Create State
                    entity.AddState(
                        State.Values.Inserted,
                        initiatorContext.Value.Id,
                        utcNow,
                        refId: Guid.NewGuid(),
                        task: "CreateCar");
                    #endregion
                }

                await _dbContext.AddRangeAsync(entities);
                var affectedCount = await _dbContext.SaveChangesAsync();
                if (affectedCount < 0)
                {
                    const string Message = "Failed to create car";
                    _logger.LogError("{Message} {@entity}", Message, entities);
                    return new ServiceResult(Message);
                }

                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to create car";
                _logger.LogError(ex, "{Message} {@entity}", Message, entities);
                return new ServiceResult(Message);
            }
        }

        public async Task<ServiceResult> UpdateCarAsync(string code, CarEntity newEntity, InitiatorServiceModel initiator)
        {
            try
            {
                _logger.LogInformation("{name} {@entity}", nameof(UpdateCarAsync), newEntity);

                var utcNow = DateTime.UtcNow;

                var initiatorContext = await _initiatorRepository.CreateOrGetInitiatorAsync(initiator.SubjectId);

                var contextModel = await _dbContext.Set<CarEntityCode>()
                    .Join(_dbContext.Set<CarEntity>(),
                        x => x.Id,
                        entity => entity.CodeId,
                        (x, entity) => new { EntityCode = x, Entity = entity })
                    .Join(_dbContext.Set<CarEntityState>().WhereNotExpired(utcNow),
                        x => x.Entity.Id,
                        state => state.EntityId,
                        (x, state) => new { x.EntityCode, x.Entity, EntityState = state })
                    .Where(x => x.EntityCode.Value == code)
                    .FirstOrDefaultAsync();

                if (contextModel == null)
                {
                    const string Message = "Car not exists";
                    _logger.LogWarning("{name} {message} {@entity}, {@code}", nameof(UpdateCarAsync), Message, newEntity, code);
                    return new ServiceResult(Message);
                }

                var existedEntityState = contextModel.EntityState;
                var existedEntity = contextModel.Entity;

                #region Create State
                newEntity.Code = existedEntity.Code;

                existedEntityState.ExpireDateTime = utcNow;

                newEntity.AddState(
                    State.Values.Updated,
                    initiatorContext.Value.Id,
                    utcNow,
                    refId: Guid.NewGuid(),
                    eTag: Guid.NewGuid(),
                    task: "UpdateCar");
                #endregion

                _dbContext.Update(existedEntity);
                await _dbContext.AddAsync(newEntity);
                var affectedCount = await _dbContext.SaveChangesAsync();
                if (affectedCount < 0)
                {
                    const string Message = "Failed to update car";
                    _logger.LogError("{Message} {@entity}", Message, newEntity);
                    return new ServiceResult(Message);
                }

                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to update car";
                _logger.LogError(ex, "{Message} {@entity}", Message, newEntity);
                return new ServiceResult(Message);
            }
        }

        public async Task<ServiceResult> DeleteCarAsync(string code, string message, InitiatorServiceModel initiator)
        {
            try
            {
                _logger.LogInformation("{name}", nameof(DeleteCarAsync));

                var utcNow = DateTime.UtcNow;

                var initiatorContext = await _initiatorRepository.CreateOrGetInitiatorAsync(initiator.SubjectId);

                var entityState = await _dbContext.Set<CarEntityCode>()
                    .Join(_dbContext.Set<CarEntity>(),
                        x => x.Id,
                        entity => entity.CodeId,
                        (x, entity) => new { EntityCode = x, Entity = entity })
                    .Join(_dbContext.Set<CarEntityState>().WhereNotExpired(utcNow),
                        x => x.Entity.Id,
                        state => state.EntityId,
                        (x, state) => new { x.EntityCode, x.Entity, EntityState = state })
                    .Where(x => x.EntityCode.Value == code)
                    .Select(x => x.EntityState)
                    .FirstOrDefaultAsync();

                if (entityState == null)
                {
                    const string Message = "Car not exists";
                    _logger.LogWarning("{name} {message}, {@code}",  nameof(DeleteCarAsync), Message, code);
                    return new ServiceResult
                    {
                        IsSuccess = true,
                        Message = Message
                    };
                }

                #region Create State
                var newEntityState = new CarEntityState
                {
                    EntityId = entityState.EntityId,
                    InitiatorId = initiatorContext.Value.Id,
                    RefId = Guid.NewGuid(),
                    ETag = Guid.NewGuid(),
                    Task = "DeleteCar",
                    StateId = (byte)State.Values.Deleted,
                    CreateDateTime = utcNow,
                    ExpireDateTime = utcNow,
                    Message = message
                };

                entityState.ExpireDateTime = utcNow;
                #endregion

                await _dbContext.AddAsync(newEntityState);
                _dbContext.Update(entityState);
                var affectedCount = await _dbContext.SaveChangesAsync();
                if (affectedCount < 0)
                {
                    const string Message = "Failed to delete car";
                    _logger.LogError("{Message}", Message);
                    return new ServiceResult(Message);
                }

                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to delete car";
                _logger.LogError(ex, "{Message}", Message);
                return new ServiceResult(Message);
            }
        }

        public async Task<PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>> QueryCarsAsync(GetCarFilter filter)
        {
            try
            {
                #region Validation
                if (filter.EnablePagination)
                {
                    if (filter.PageNumber != null && filter.PageNumber.Value < 1)
                    {
                        const string Message = $"{nameof(filter.PageNumber)} must be more than 0.";
                        _logger.LogError("{Message} {@filter}", Message, filter);
                        return new PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>
                        {
                            Message = Message
                        };
                    }

                    if (filter.PageSize != null)
                    {
                        if (filter.PageSize.Value < 1)
                        {
                            const string Message = $"{nameof(filter.PageSize)} must be more than 0.";
                            _logger.LogError("{name} {Message}, {@filter}", nameof(QueryCarsAsync), Message, filter);
                            return new PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>
                            {
                                IsSuccess = true,
                                Message = Message
                            };
                        }
                        if (filter.PageSize.Value > 100)
                        {
                            const string Message = $"{nameof(filter.PageSize)} must be less or equal to 100.";
                            _logger.LogError("{name} {Message}, {@filter}", nameof(QueryCarsAsync), Message, filter);
                            return new PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>
                            {
                                IsSuccess = true,
                                Message = Message
                            };
                        }
                    }
                }
                #endregion

                #region Query
                var utcNow = DateTime.UtcNow;

                var query = _dbContext.Set<CarEntityCode>()
                    .Join(_dbContext.Set<CarEntity>().Include(x => x.Code),
                        x => x.Id,
                        entity => entity.CodeId,
                        (x, entity) => new { EntityCode = x, Entity = entity })
                    .Join(_dbContext.Set<CarEntityState>().WhereNotExpired(utcNow),
                        x => x.Entity.Id,
                        state => state.EntityId,
                        (x, state) => new { x.EntityCode, x.Entity, EntityState = state });
                #endregion

                #region Filter
                if (filter.Codes != null && filter.Codes.Any())
                {
                    query = query.Where(x => filter.Codes.Contains(x.EntityCode.Value));
                }

                if (!string.IsNullOrWhiteSpace(filter.SearchValue))
                {
                    query = query.Where(x => x.Entity.ModelName.Contains(filter.SearchValue) || x.Entity.CarName.Contains(filter.SearchValue));
                }

                if (!string.IsNullOrWhiteSpace(filter.OwnerName))
                {
                    query = query.Where(x => x.Entity.OwnerName.Contains(filter.OwnerName));
                }

                if (filter.OwnersPriceFrom.HasValue)
                {
                    query = query.Where(x => filter.OwnersPriceFrom.Value <= x.Entity.OwnersPrice);
                }

                if (filter.OwnersPriceTo.HasValue)
                {
                    query = query.Where(x => filter.OwnersPriceTo.Value >= x.Entity.OwnersPrice);
                }

                if (filter.DealersPriceFrom.HasValue)
                {
                    query = query.Where(x => filter.DealersPriceFrom.Value <= x.Entity.DealersPrice);
                }

                if (filter.DealersPriceTo.HasValue)
                {
                    query = query.Where(x => filter.DealersPriceTo.Value >= x.Entity.DealersPrice);
                }

                if (filter.SellFinalPriceFrom.HasValue)
                {
                    query = query.Where(x => filter.SellFinalPriceFrom.Value <= x.Entity.SellFinalPrice);
                }

                if (filter.SellFinalPriceTo.HasValue)
                {
                    query = query.Where(x => filter.SellFinalPriceTo.Value >= x.Entity.SellFinalPrice);
                }

                if (!string.IsNullOrEmpty(filter.DealerNotes))
                {
                    query = query.Where(x => x.Entity.DealerNotes.Contains(filter.DealerNotes));
                }

                if (filter.DateTimeCreatedFrom.HasValue)
                {
                    query = query.Where(x => filter.DateTimeCreatedFrom.Value <= x.EntityState.CreateDateTime);
                }

                if (filter.DateTimeCreatedTo.HasValue)
                {
                    query = query.Where(x => filter.DateTimeCreatedTo.Value >= x.EntityState.CreateDateTime);
                }
                #endregion

                #region Order by
                const string CarNameField = "CarName";
                const string ModelNameField = "ModelName";
                const string OwnerNameField = "OwnerName";
                const string DealersPriceField = "DealersPrice";
                const string OwnersPriceField = "OwnersPrice";
                const string DateTimeCreatedField = "DateTimeCreated";

                if (filter.OrderDirection == OrderDirection.Asc)
                {
                    switch (filter.OrderBy)
                    {
                        case CarNameField:
                            query = query.OrderBy(x => x.Entity.CarName);
                            break;
                        case ModelNameField:
                            query = query.OrderBy(x => x.Entity.ModelName);
                            break;
                        case OwnerNameField:
                            query = query.OrderBy(x => x.Entity.OwnerName);
                            break;
                        case DealersPriceField:
                            query = query.OrderBy(x => x.Entity.DealersPrice);
                            break;
                        case OwnersPriceField:
                            query = query.OrderBy(x => x.Entity.OwnersPrice);
                            break;
                        case DateTimeCreatedField:
                        default:
                            query = query.OrderBy(x => x.EntityState.CreateDateTime);
                            break;
                    }
                }
                else
                {
                    switch (filter.OrderBy)
                    {
                        case CarNameField:
                            query = query.OrderByDescending(x => x.Entity.CarName);
                            break;
                        case ModelNameField:
                            query = query.OrderByDescending(x => x.Entity.ModelName);
                            break;
                        case OwnerNameField:
                            query = query.OrderByDescending(x => x.Entity.OwnerName);
                            break;
                        case DealersPriceField:
                            query = query.OrderByDescending(x => x.Entity.DealersPrice);
                            break;
                        case OwnersPriceField:
                            query = query.OrderByDescending(x => x.Entity.OwnersPrice);
                            break;
                        case DateTimeCreatedField:
                        default:
                            query = query.OrderByDescending(x => x.EntityState.CreateDateTime);
                            break;
                    }
                }
                #endregion

                #region Include total count
                int? totalCount = null;
                if (filter.IncludeTotalCount)
                {
                    totalCount = await query.CountAsync();
                }
                #endregion

                #region Pagination
                if (filter.PageSize != null && filter.EnablePagination)
                {
                    var countToSkip = (filter.PageNumber.Value - 1) * filter.PageSize.Value;
                    var countToTake = filter.PageSize.Value + 1;

                    query = query
                        .Skip(countToSkip)
                        .Take(countToTake);
                }
                #endregion

                #region Prepare result
                var resultedList = await query.Select(x => x.Entity).ToListAsync();

                var result = new PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>
                {
                    IsSuccess = true,
                    Value = filter.PageSize != null && filter.EnablePagination
                        ? resultedList.Take(filter.PageSize.Value)
                        : resultedList,
                    TotalCount = totalCount
                };

                if (filter.PageNumber == null || !filter.EnablePagination)
                {
                    return result;
                }
                #endregion

                #region Prepare prev and next filters
                if (filter.PageNumber.Value > 1)
                {
                    result.Prev = new GetCarFilter
                    {
                        Codes = filter.Codes,
                        SearchValue = filter.SearchValue,
                        OwnerName = filter.OwnerName,
                        OwnersPriceFrom = filter.OwnersPriceFrom,
                        OwnersPriceTo = filter.OwnersPriceTo,
                        DealersPriceFrom = filter.DealersPriceFrom,
                        DealersPriceTo = filter.DealersPriceTo,
                        SellFinalPriceFrom = filter.SellFinalPriceFrom,
                        SellFinalPriceTo = filter.SellFinalPriceTo,
                        DateTimeCreatedFrom = filter.DateTimeCreatedFrom,
                        DateTimeCreatedTo = filter.DateTimeCreatedTo,
                        DealerNotes = filter.DealerNotes,
                        EnablePagination = filter.EnablePagination,
                        PageNumber = filter.PageNumber.Value - 1,
                        PageSize = filter.PageSize,
                        IncludeTotalCount = filter.IncludeTotalCount,
                        OrderDirection = filter.OrderDirection,
                        OrderBy = filter.OrderBy
                    };
                }

                if (resultedList.Count > filter.PageSize)
                {
                    result.Next = new GetCarFilter
                    {
                        Codes = filter.Codes,
                        SearchValue = filter.SearchValue,
                        OwnerName = filter.OwnerName,
                        OwnersPriceFrom = filter.OwnersPriceFrom,
                        OwnersPriceTo = filter.OwnersPriceTo,
                        DealersPriceFrom = filter.DealersPriceFrom,
                        DealersPriceTo = filter.DealersPriceTo,
                        SellFinalPriceFrom = filter.SellFinalPriceFrom,
                        SellFinalPriceTo = filter.SellFinalPriceTo,
                        DateTimeCreatedFrom = filter.DateTimeCreatedFrom,
                        DateTimeCreatedTo = filter.DateTimeCreatedTo,
                        DealerNotes = filter.DealerNotes,
                        EnablePagination = filter.EnablePagination,
                        PageNumber = filter.PageNumber.Value + 1,
                        PageSize = filter.PageSize,
                        IncludeTotalCount = filter.IncludeTotalCount,
                        OrderDirection = filter.OrderDirection,
                        OrderBy = filter.OrderBy
                    };
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                const string Message = "Failed to create car";
                _logger.LogError(ex, "{Message} {@filter}", Message, filter);
                return new PaginatedServiceResultWrapper<IEnumerable<CarEntity>, GetCarFilter>
                {
                    Message = Message
                };
            }
        }

        public async Task<ServiceResultWrapper<GetCarsMetricsResponse>> QueryMetricsAsync()
        {
            try
            {
                _logger.LogInformation("{name}", nameof(QueryMetricsAsync));

                #region Query
                var utcNow = DateTime.UtcNow;

                var query = _dbContext.Set<CarEntityCode>()
                    .Join(_dbContext.Set<CarEntity>().Include(x => x.Code),
                        x => x.Id,
                        entity => entity.CodeId,
                        (x, entity) => new { EntityCode = x, Entity = entity })
                    .Join(_dbContext.Set<CarEntityState>().WhereNotExpired(utcNow),
                        x => x.Entity.Id,
                        state => state.EntityId,
                        (x, state) => new { x.EntityCode, x.Entity, EntityState = state });
                #endregion

                var totalCars = await query.CountAsync();
                var totalPricing = await query.SumAsync(x => x.Entity.DealersPrice);

                var result = new GetCarsMetricsResponse
                {
                    TotalCars = totalCars,
                    TotalPricing = totalPricing
                };

                return new ServiceResultWrapper<GetCarsMetricsResponse>(result);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to get metrics";
                _logger.LogError(ex, "{Message}", Message);
                return new ServiceResultWrapper<GetCarsMetricsResponse>(Message);
            }
        }
    }
}
