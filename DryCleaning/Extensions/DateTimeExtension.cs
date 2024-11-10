namespace DryCleaning.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="DateTime"/> type to facilitate 
    /// the conversion of a <see cref="DateTime"/> to a tuple containing a <see cref="DateOnly"/> 
    /// and a <see cref="TimeOnly"/>.
    public static class DateTimeExtension
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to a tuple containing a <see cref="DateOnly"/> 
        /// (representing the date) and a <see cref="TimeOnly"/> (representing the time).
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> value to convert.</param>
        /// <returns>A tuple where the first item is a <see cref="DateOnly"/> representing the date, 
        /// and the second item is a <see cref="TimeOnly"/> representing the time.</returns>
        public static (DateOnly, TimeOnly) ToDateAndHours(this DateTime dateTime) => (DateOnly.FromDateTime(dateTime), TimeOnly.FromDateTime(dateTime));

    }
}
