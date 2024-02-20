using Majestic.WarehouseService.Models.Misc;
using Newtonsoft.Json;

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
        public decimal OwnersPrice { get; set; }

        /// <summary>
        /// Search by dealers price
        /// </summary>
        public decimal DealersPrice { get; set; }

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
        [JsonProperty("pageNumber")]
        public int? PageNumber { get; set; } = 1;

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; } = 10;
        #endregion

        #region Order
        [JsonProperty("orderDirection")]
        public OrderDirection OrderDirection { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        /// <remarks>
        /// Possible values: CarName, ModelName, OwnerName, DealersPrice, DateTimeCreated
        /// </remarks>
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }
        #endregion
    }
}
