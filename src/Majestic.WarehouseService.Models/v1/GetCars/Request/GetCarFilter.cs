using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Models.v1.GetCars.Request
{
    public class GetCarFilter
    {
        #region Fields
        /// <summary>
        /// Search by Codes
        /// </summary>
        public ICollection<string> Codes { get; set; }

        /// <summary>
        /// Search by Model Name or Car Name
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Serach by owner name
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Search by owners price
        /// </summary>
        public decimal? OwnersPriceFrom { get; set; }

        /// <summary>
        /// Search by owners price
        /// </summary>
        public decimal? OwnersPriceTo { get; set; }

        /// <summary>
        /// Search by dealers price
        /// </summary>
        public decimal? DealersPriceFrom { get; set; }

        /// <summary>
        /// Search by dealers price
        /// </summary>
        public decimal? DealersPriceTo { get; set; }

        /// <summary>
        /// Search by dealers notes
        /// </summary>
        public string DealerNotes { get; set; }

        /// <summary>
        /// Search by date time created from
        /// </summary>
        public DateTime? DateTimeCreatedFrom { get; set; }

        /// <summary>
        /// Search by date time created to
        /// </summary>
        public DateTime? DateTimeCreatedTo { get; set; }
        #endregion

        #region Pagination
        /// <summary>
        /// Set to true if you need to enable pagination
        /// </summary>
        public bool EnablePagination { get; set; } = true;

        /// <summary>
        /// Page number
        /// </summary>
        public int? PageNumber { get; set; } = 1;

        /// <summary>
        /// Page size
        /// </summary>
        public int? PageSize { get; set; } = 10;
        #endregion

        #region Order
        /// <summary>
        /// Order direction
        /// </summary>
        public OrderDirection OrderDirection { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        /// <remarks>
        /// Possible values: CarName, ModelName, OwnerName, DealersPrice, DateTimeCreated
        /// </remarks>
        public string OrderBy { get; set; }
        #endregion

        #region Include
        /// <summary>
        /// Set to true if you need to get total count
        /// </summary>
        public bool IncludeTotalCount { get; set; } = false;
        #endregion
    }
}
