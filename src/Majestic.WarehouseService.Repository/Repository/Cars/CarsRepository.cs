using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.GetCars.Request;
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

        public async Task<ServiceResult> CreateCarAsync(CarEntity entity, InitiatorServiceModel initiator)
        {
            try
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
                var initiatorContext = await _initiatorRepository.CreateOrGetInitiatorAsync(initiator.SubjectId);

                entity.AddState(
                    State.Values.Inserted,
                    initiatorContext.Value.Id,
                    refId: Guid.NewGuid());
                #endregion

                await _dbContext.AddAsync(entity);
                var affectedCount = await _dbContext.SaveChangesAsync();
                if (affectedCount < 0)
                {
                    const string Message = "Failed to create car";
                    _logger.LogError("{Message} {@entity}", Message, entity);
                    return new ServiceResult(Message);
                }

                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                const string Message = "Failed to create car";
                _logger.LogError(ex, "{Message} {@entity}", Message, entity);
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
                    .Join(_dbContext.Set<CarEntity>(),
                        x => x.Id,
                        entity => entity.CodeId,
                        (x, entity) => new { EntityCode = x, Entity = entity })
                    .Join(_dbContext.Set<CarEntityState>(),
                        x => x.Entity.Id,
                        state => state.EntityId,
                        (x, state) => new { x.EntityCode, x.Entity, EntityState = state })
                    .Where(x => x.EntityState.CreateDateTime <= utcNow)
                    .Where(x => x.EntityState.ExpireDateTime == null || x.EntityState.ExpireDateTime > utcNow);
                #endregion

                #region Filter
                if (filter.Codes != null && filter.Codes.Any())
                {
                    query = query.Where(x => filter.Codes.Contains(x.EntityCode.Value));
                }

                if (!string.IsNullOrWhiteSpace(filter.SearchValue))
                {
                    var searchValue = filter.SearchValue.ToLower();
                    query = query.Where(
                        x => x.Entity.ModelName.ToLower().Contains(searchValue) || x.Entity.CarName.ToString().Contains(searchValue));
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
    }
}
