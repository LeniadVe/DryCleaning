using DryCleaning.DTO;
using LanguageExt.Common;

namespace DryCleaning.Interfaces
{
    public interface IShopService
    {
        Result<string> Get(int minutes, DateTime startDate);

        Result<bool> UpdateWeek(WorkHours? newWorkHours = null, List<DayOfWeek>? days = null);

        Result<bool> UpdateWorkingHours(DayOfWeek dayOfWeek, WorkHours? newWorkHours = null);

        Result<bool> UpdateWorkingHours(KeyValuePair<DayOfWeek, WorkHours> day, WorkHours? newWorkHours = null);

        Result<WorkHours> AddDate(DateOnly date, WorkHours? workHours = null);

        Result<bool> AddDates(List<DateOnly> dates, WorkHours? workHours = null);
    }
}
