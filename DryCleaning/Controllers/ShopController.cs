using DryCleaning.DTO;
using DryCleaning.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaning.Controllers
{
    /// <summary>
    /// API Controller for managing shop schedules, including setting open/close hours and calculating service completion times.
    /// </summary>
    /// <param name="shopService">Service for managing shop scheduling data and business logic.</param>
    [ApiController]
    public class ShopController(IShopService shopService) : Controller
    {
        private readonly IShopService _shopService = shopService;
        private readonly char commaSeparator = ',';

        /// <summary>
        /// Sets the opening and closing hours for all weekdays uniformly.
        /// </summary>
        /// <param name="openingHour">The opening hour in "HH:mm" format.</param>
        /// <param name="closingHour">The closing hour in "HH:mm" format.</param>
        /// <returns>An HTTP response indicating success or failure in updating the schedule.</returns>
        [HttpPost("days-schedule")]
        public IActionResult DaysSchedule(string openingHour, string closingHour)
        {
            if (!TimeOnly.TryParse(openingHour, out TimeOnly openingTime))
                return BadRequest($"The opening hour format is incorrect: {openingHour}. It must be 'HH:mm'." );
            if (!TimeOnly.TryParse(closingHour, out TimeOnly closingTime))
                return BadRequest($"The closing hour format is incorrect: {closingHour}. It must be 'HH:mm'." );
            if (closingTime <= openingTime)
                return BadRequest($"The closing hour ({closingTime}) cannot be equal to or earlier than the opening hour ({openingTime})." );

            var result = _shopService.UpdateWeek(new WorkHours { Open = openingTime, Close = closingTime });

            return result.Match(
                    x => x ? Ok("Days schedule updated successfully.") : ValidationProblem("Days cannot be scheduled."),
                    x =>  StatusCode(x.HResult, $"Message: Days cannot be scheduled. {Environment.NewLine}Error: {x.Message}"));
        }

        /// <summary>
        /// Sets the opening and closing hours for a specific day of the week.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week (e.g., "Monday", "Tuesday").</param>
        /// <param name="openingHour">The opening hour in "HH:mm" format.</param>
        /// <param name="closingHour">The closing hour in "HH:mm" format.</param>
        /// <returns>An HTTP response indicating success or failure in updating the schedule for the specified day.</returns>
        [HttpPost("days")]
        public IActionResult Days(string dayOfWeek, string openingHour, string closingHour)
        {
            if (!Enum.TryParse(dayOfWeek, true, out DayOfWeek day))
                return BadRequest($"The day of the week '{dayOfWeek}' is not valid. Please provide a valid day (e.g., 'Monday', 'Tuesday')." );
            if (!TimeOnly.TryParse(openingHour, out TimeOnly openingTime))
                return BadRequest($"The opening hour format is incorrect: {openingHour}. It must be 'HH:mm'." );
            if (!TimeOnly.TryParse(closingHour, out TimeOnly closingTime))
                return BadRequest($"The closing hour format is incorrect: {closingHour}. It must be 'HH:mm'." );
            if (closingTime <= openingTime)
                return BadRequest($"The closing hour ({closingTime}) cannot be equal to or earlier than the opening hour ({openingTime}).");

            var result = _shopService.UpdateWorkingHours(day, new WorkHours { Open = openingTime, Close = closingTime });

            return result.Match(
                    x => x ? Ok("Days schedule updated successfully.") : ValidationProblem("Days cannot be scheduled."),
                    x => StatusCode(x.HResult, $"Message: Days cannot be scheduled. {Environment.NewLine}Error: {x.Message}"));
        }

        /// <summary>
        /// Sets the opening and closing hours for a specific date.
        /// </summary>
        /// <param name="date">The specific date in "yyyy-MM-dd" format.</param>
        /// <param name="openingHour">The opening hour in "HH:mm" format.</param>
        /// <param name="closingHour">The closing hour in "HH:mm" format.</param>
        /// <returns>An HTTP response indicating success or failure in updating the schedule for the specified date.</returns>
        [HttpPost("date")]
        public IActionResult Dates(string date, string openingHour, string closingHour)
        {
            if (!DateOnly.TryParse(date, out DateOnly day))
                return BadRequest($"The date format is incorrect: {date}. It must be in 'yyyy-MM-dd' format." );
            if (!TimeOnly.TryParse(openingHour, out TimeOnly openingTime))
                return BadRequest($"The opening hour format is incorrect: {openingHour}. It must be 'HH:mm'." );
            if (!TimeOnly.TryParse(closingHour, out TimeOnly closingTime))
                return BadRequest($"The closing hour format is incorrect: {closingHour}. It must be 'HH:mm'." );
            if (closingTime <= openingTime)
                return BadRequest($"The closing hour ({closingTime}) cannot be equal to or earlier than the opening hour ({openingTime})." );

            var result = _shopService.AddDate(day, new WorkHours { Open = openingTime, Close = closingTime });

            return result.Match(
                    x => x != null ? Ok("Dates added successfully.") : ValidationProblem("Dates cannot be scheduled."),
                    x => StatusCode(x.HResult, $"Message: Dates cannot be scheduled. {Environment.NewLine}Error: {x.Message}"));
        }

        /// <summary>
        /// Marks specific days of the week as closed.
        /// </summary>
        /// <param name="daysOfWeek">A comma-separated list of days of the week (e.g., "Sunday, Wednesday").</param>
        /// <returns>An HTTP response indicating success or failure in updating the closed days schedule.</returns>
        [HttpPost("days-close")]
        public IActionResult DaysClose(string daysOfWeek)
        {
            var days = new List<DayOfWeek>();
            foreach (var dayOfWeek in daysOfWeek.Split(commaSeparator))
            {
                if (!Enum.TryParse(dayOfWeek, true, out DayOfWeek day))
                    return BadRequest($"The day of the week '{dayOfWeek}' is not valid. Please provide a valid day (e.g., 'Monday', 'Tuesday')." );
                days.Add(day);
            }

            var result = _shopService.UpdateWeek(days: days);

            return result.Match(
                    x => x ? Ok("Days schedule updated successfully.") : ValidationProblem("Days cannot be scheduled."),
                    x => StatusCode(x.HResult, $"Message: Days cannot be scheduled. {Environment.NewLine}Error: {x.Message}"));

        }

        /// <summary>
        /// Marks specific dates as closed.
        /// </summary>
        /// <param name="dates">A comma-separated list of dates in "yyyy-MM-dd" format.</param>
        /// <returns>An HTTP response indicating success or failure in updating the closed dates schedule.</returns>
        [HttpPost("dates-close")]
        public IActionResult DatesClose(string dates)
        {
            var dateList = new List<DateOnly>();
            foreach (var date in dates.Split(commaSeparator))
            {
                if (!DateOnly.TryParse(date, out DateOnly day))
                    return BadRequest($"The date format is incorrect: {date}. It must be in 'yyyy-MM-dd' format." );
                dateList.Add(day);
            }
            var result = _shopService.AddDates(dateList);

            return result.Match(
                    x => x ? Ok("Dates added successfully.") : ValidationProblem("Dates cannot be scheduled."),
                    x => StatusCode(x.HResult, $"Message: Dates cannot be scheduled. {Environment.NewLine}Error: {x.Message}"));
        }

        /// <summary>
        /// Calculates the estimated completion date and time for a service based on the given duration and start date.
        /// </summary>
        /// <param name="minutes">The duration of the service in minutes.</param>
        /// <param name="date">The start date and time in "yyyy-MM-dd HH:mm" format.</param>
        /// <returns>The calculated completion date and time, or an error message if input is invalid.</returns>
        [HttpGet("schedule-calculator")]
        public IActionResult Get(int minutes, string date)
        {
            if (minutes < 0)
                return BadRequest("The duration in minutes cannot be negative. Please provide a valid positive number." );
            if (!DateTime.TryParse(date, out DateTime dateTime))
                return BadRequest($"The date and time format is incorrect: {date}. It must be in 'yyyy-MM-dd HH:mm' format." );

            var result = _shopService.Get(minutes, dateTime);

            return result.Match(x => Ok(x), x => StatusCode(x.HResult, x.Message));
        }
    }
}
