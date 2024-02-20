namespace Majestic.WarehouseService.Models.Misc
{
    public class ServiceResult
    {
        /// <summary>
        /// Indicates whether we have success of not
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Service message, may contain error information
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Timestamp when error occured
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Dictionary for returning set of errors, e.g. when validating model
        /// </summary>
        public IDictionary<string, IList<string>> Data { get; set; }

        /// <summary>
        /// Create ServiceResult without message
        /// </summary>
        public ServiceResult(bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Create unsuccessful ServiceResult with message
        /// </summary>
        public ServiceResult(string message) : this()
        {
            Message = message;
        }

        public void AddData(string key, string value)
        {
            if (Data == null)
            {
                Data = new Dictionary<string, IList<string>>();
            }

            if (Data.TryGetValue(key, out var list))
            {
                list.Add(value);
                return;
            }

            list = new List<string> { value };
            Data.Add(key, list);
        }
    }
}
