using Newtonsoft.Json;

namespace Majestic.WarehouseService.Models.Misc
{
    public class PaginatedServiceResultWrapper<TReturnValue, TPageModel>
        : ServiceResultWrapper<TReturnValue>
    {
        [JsonProperty(PropertyName = "prev")]
        public TPageModel Prev { get; set; }

        [JsonProperty(PropertyName = "next")]
        public TPageModel Next { get; set; }

        [JsonProperty(PropertyName = "totalCount")]
        public int? TotalCount { get; set; }
    }
}
