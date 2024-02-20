namespace Majestic.WarehouseService.Models.Misc
{
    public class ServiceResultWrapper<T> : ServiceResult
    {
        /// <summary>
        /// Generic value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Create unsuccessful ServiceResultWrapper without value and message
        /// </summary>
        public ServiceResultWrapper() : base()
        {
        }

        /// <summary>
        /// Create unsuccessful ServiceResultWrapper with message, but without value
        /// </summary>
        public ServiceResultWrapper(string message) : base(message)
        {
        }

        /// <summary>
        /// Create unsuccessful ServiceResultWrapper with message and value
        /// </summary>
        public ServiceResultWrapper(string message, T value) : this(message)
        {
            Value = value;
        }

        /// <summary>
        /// Create successful ServiceResultWrapper without message, but with value
        /// </summary>
        public ServiceResultWrapper(T value) : this()
        {
            IsSuccess = true;
            Value = value;
        }
    }
}
