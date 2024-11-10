namespace DryCleaning.DTO
{
    /// <summary>
    /// Represents the working hours for a specific period, including the opening and closing times.
    /// </summary>
    /// <remarks>
    /// This class holds two properties, <see cref="Open"/> and <see cref="Close"/>, 
    /// which define the start and end times of a work schedule. Both properties are nullable 
    /// and default to <see cref="TimeOnly.MinValue"/> and <see cref="TimeOnly.MaxValue"/> respectively,
    /// allowing for flexible scheduling.
    /// </remarks>
    public class WorkHours
    {
        /// <summary>
        /// Gets or sets the opening time for the work period.
        /// </summary>
        /// <remarks>
        /// If not set explicitly, defaults to <see cref="TimeOnly.MinValue"/> (00:00:00).
        /// </remarks>
        public TimeOnly? Open { get; set; } = TimeOnly.MinValue;

        /// <summary>
        /// Gets or sets the closing time for the work period.
        /// </summary>
        /// <remarks>
        /// If not set explicitly, defaults to <see cref="TimeOnly.MaxValue"/> (23:59:59).
        /// </remarks>
        public TimeOnly? Close { get; set; } = TimeOnly.MaxValue;
    }
}
