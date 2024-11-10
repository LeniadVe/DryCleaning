using DryCleaning.DTO;

namespace DryCleaning.Factories
{
    /// <summary>
    /// Factory class for creating and initializing instances of the <see cref="Shop"/> class.
    /// </summary>
    public static class ShopFactory
    {
        /// <summary>
        /// Creates and returns an instance of <see cref="Shop"/> with pre-initialized values.
        /// Each day of the week is set with a default <see cref="WorkHours"/> entry, 
        /// ensuring a complete schedule structure from the start.
        /// </summary>
        /// <returns>An initialized <see cref="Shop"/> instance with default values for each day of the week.</returns>
        public static Shop InitializeWeek(this Shop shop)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                shop.Week.TryAdd(day, new WorkHours());
            }
            return shop;
        }
    }
}
