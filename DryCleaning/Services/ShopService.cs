using DryCleaning.DTO;
using DryCleaning.Extensions;
using DryCleaning.Factories;
using DryCleaning.Interfaces;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace DryCleaning.Services
{
    /// <summary>
    /// Provides business logic for managing the store's schedule, including opening and closing hours
    /// for specific dates and days of the week.
    /// </summary>
    /// <remarks>
    /// This service allows updating, adding, and retrieving work hours for various days and dates.
    /// It also calculates service completion times based on store hours.
    /// </remarks>
    public class ShopService : IShopService
    {
        private readonly Shop _shop = new Shop().InitializeWeek();
        private const int oneDay = 1;
        private static WorkHours closeWorkHours { get => new() { Open = null, Close = null }; }

        /// <summary>
        /// Calculates the guaranteed completion date and time for a specified service duration, 
        /// based on business hours starting from the provided date and time.
        /// </summary>
        /// <param name="minutes">Duration in minutes for service completion.</param>
        /// <param name="startDate">The start date and time for calculating the completion date.</param>
        /// <returns>A <see cref="Result{T}"/> containing the completion date and time formatted as "ddd MMM dd HH:mm:ss yyyy".</returns>
        public Result<string> Get(int minutes, DateTime startDate)
        {
            (var date, var hour) = startDate.ToDateAndHours();

            if (!VerifySchedule(date))
                return new(new ValidationException($"The shop doesn't have opening hours for the indicated date."));

            var resultDate = CalculateFinishDate(minutes, date, hour);
            return resultDate.ToString("ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Recursively calculates the end date and time for a specified service duration
        /// considering business hours. Advances to the next business day if necessary.
        /// </summary>
        /// <param name="minutes">Remaining duration in minutes to complete the service.</param>
        /// <param name="date">The current date in the calculation.</param>
        /// <param name="hour">The current time on the specified date in the calculation.</param>
        /// <returns>A <see cref="DateTime"/> representing the calculated completion date and time.</returns>
        private DateTime CalculateFinishDate(int minutes, DateOnly date, TimeOnly hour)
        {
            if (_shop.Dates.TryGetValue(date, out var workHours) && (workHours.Open == null || workHours.Close == null))
                return CalculateFinishDate(minutes, date.AddDays(oneDay), TimeOnly.MinValue);
            if (workHours == null && _shop.Week.TryGetValue(date.DayOfWeek, out workHours) && (workHours.Open == null || workHours.Close == null))
                return CalculateFinishDate(minutes, date.AddDays(oneDay), TimeOnly.MinValue);

            hour = hour < workHours!.Open ? workHours.Open.Value.AddMinutes(minutes) : hour.AddMinutes(minutes);
            if (hour > workHours.Close)
                return CalculateFinishDate((int)(hour - workHours.Close).Value.TotalMinutes, date.AddDays(oneDay), TimeOnly.MinValue);

            return date.ToDateTime(hour);
        }

        /// <summary>
        /// Verifies if the store has any scheduled hours.
        /// </summary>
        /// <param name="date">The date to check against the store's schedule.</param>
        /// <returns>
        /// Returns <c>true</c> if there are valid hours for any day in the weekly schedule
        /// or if there are specific dates with hours after the given date; otherwise, <c>false</c>.
        /// </returns>
        private bool VerifySchedule(DateOnly date) => _shop.Week.Any(x => x.Value.Open != null && x.Value.Close != null) || _shop.Dates.Any(x => x.Value.Open != null && x.Value.Close != null && x.Key > date);


        /// <summary>
        /// Updates the business hours for specified days of the week, or all days if no specific days are provided.
        /// </summary>
        /// <param name="newWorkHours">The new work hours to apply to the specified days.</param>
        /// <param name="days">The days of the week to update. Updates all days if <c>null</c>.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether the update was successful for all specified days.</returns>
        public Result<bool> UpdateWeek(WorkHours? newWorkHours = null, List<DayOfWeek>? days = null)
        {
            foreach (var item in _shop.Week.Where(item => days == null || days.Contains(item.Key)))
            {
                var result = UpdateWorkingHours(item, newWorkHours);
                if (result.IsFaulted || result.Match(x => !x, x => true))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Updates the business hours for a specific day of the week in the store's schedule.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week to update.</param>
        /// <param name="newWorkHours">The new work hours for the specified day. If <c>null</c>, sets the day as closed.</param>
        /// <returns>A <see cref="Result{T}"/> indicating if the update was successful for the specified day.</returns>
        public Result<bool> UpdateWorkingHours(DayOfWeek dayOfWeek, WorkHours? newWorkHours = null) =>
            _shop.Week.TryGetValue(dayOfWeek, out var workHours) && _shop.Week.TryUpdate(dayOfWeek, newWorkHours ?? closeWorkHours, workHours);

        /// <summary>
        /// Updates the business hours for a given key-value pair representing a day and its current work hours.
        /// </summary>
        /// <param name="day">The day and its current work hours to update.</param>
        /// <param name="newWorkHours">The new work hours for the specified day. If <c>null</c>, sets the day as closed.</param>
        /// <returns>A <see cref="Result{T}"/> indicating if the update was successful.</returns>
        public Result<bool> UpdateWorkingHours(KeyValuePair<DayOfWeek, WorkHours> day, WorkHours? newWorkHours = null) =>
            _shop.Week.TryUpdate(day.Key, newWorkHours ?? closeWorkHours, day.Value);

        /// <summary>
        /// Adds or updates the business hours for a specific date in the store's calendar.
        /// </summary>
        /// <param name="date">The specific date to add or update.</param>
        /// <param name="workHours">The work hours for the date. If <c>null</c>, the date is considered closed.</param>
        /// <returns>A <see cref="Result{T}"/> containing the added or updated <see cref="WorkHours"/> for the date.</returns>
        public Result<WorkHours> AddDate(DateOnly date, WorkHours? workHours = null) =>
            _shop.Dates.AddOrUpdate(date, workHours ?? closeWorkHours, (d, w) => workHours ?? closeWorkHours);

        /// <summary>
        /// Adds or updates business hours for multiple specific dates in the store's calendar.
        /// </summary>
        /// <param name="dates">A list of dates to add or update.</param>
        /// <param name="workHours">The work hours for each specified date. If <c>null</c>, each date is considered closed.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether all specified dates were successfully added or updated.</returns>
        public Result<bool> AddDates(List<DateOnly> dates, WorkHours? workHours = null)
        {
            foreach (var date in dates)
            {
                var result = AddDate(date, workHours);
                if (result.IsFaulted || result.Match(x => x == null, x => true))
                    return false;
            }
            return true;
        }
    }
}
