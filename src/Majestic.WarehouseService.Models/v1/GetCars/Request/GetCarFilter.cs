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
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ICollection<string> Codes { get; set; }

        /// <summary>
        /// Search by Model Name or Car Name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SearchValue { get; set; }

        /// <summary>
        /// Serach by owner name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OwnerName { get; set; }

        /// <summary>
        /// Search by owners price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? OwnersPriceFrom { get; set; }

        /// <summary>
        /// Search by owners price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? OwnersPriceTo { get; set; }

        /// <summary>
        /// Search by dealers price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? DealersPriceFrom { get; set; }

        /// <summary>
        /// Search by dealers price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? DealersPriceTo { get; set; }

        /// <summary>
        /// Search by sell final price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? SellFinalPriceFrom { get; set; }

        /// <summary>
        /// Search by sell final price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? SellFinalPriceTo { get; set; }

        /// <summary>
        /// Search by dealers notes
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DealerNotes { get; set; }

        /// <summary>
        /// Search by date time created from
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? DateTimeCreatedFrom { get; set; }

        /// <summary>
        /// Search by date time created to
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? DateTimeCreatedTo { get; set; }
        #endregion

        #region Pagination
        /// <summary>
        /// Set to true if you need to enable pagination
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool EnablePagination { get; set; } = true;

        /// <summary>
        /// Page number
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PageNumber { get; set; } = 1;

        /// <summary>
        /// Page size
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PageSize { get; set; } = 10;
        #endregion

        #region Order
        /// <summary>
        /// Order direction
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public OrderDirection OrderDirection { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        /// <remarks>
        /// Possible values: CarName, ModelName, OwnerName, DealersPrice, DateTimeCreated
        /// </remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OrderBy { get; set; }
        #endregion

        #region Include
        /// <summary>
        /// Set to true if you need to get total count
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IncludeTotalCount { get; set; } = false;
        #endregion
    }
}
