using System.Collections.Concurrent;

namespace DryCleaning.DTO
{
    /// <summary>
    /// Represents the schedule configuration for a shop, including working hours for each day of the week
    /// and specific dates with custom opening and closing times.
    /// </summary>
    /// <remarks>
    /// The <see cref="Week"/> property contains daily hours keyed by day of the week, while 
    /// <see cref="Dates"/> allows specific date-based overrides for opening and closing times. 
    /// Uses <see cref="ConcurrentDictionary{TKey, TValue}"/> to ensure thread-safe access and modification.
    /// </remarks>
    public partial class Shop
    {
        /// <summary>
        /// Represents the working hours for each day of the week, stored as a dictionary where the key is the day of the week
        /// and the value is the <see cref="WorkHours"/> for that day.
        /// </summary>
        /// <remarks>
        /// This property is used to define the typical working hours of the shop, which are the same for each week.
        /// The dictionary is thread-safe, allowing concurrent access and modification.
        /// </remarks>
        public ConcurrentDictionary<DayOfWeek, WorkHours> Week { get; set; } = new ConcurrentDictionary<DayOfWeek, WorkHours>();

        /// <summary>
        /// Represents the custom working hours for specific dates, stored as a dictionary where the key is the specific date
        /// and the value is the <see cref="WorkHours"/> for that date.
        /// </summary>
        /// <remarks>
        /// This property allows setting specific working hours for dates that may differ from the regular weekly schedule.
        /// The dictionary is thread-safe, allowing concurrent access and modification.
        /// </remarks>
        public ConcurrentDictionary<DateOnly, WorkHours> Dates { get; set; } = new ConcurrentDictionary<DateOnly, WorkHours>();                
    }
}
