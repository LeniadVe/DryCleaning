# DryCleaning

Endpoints:

    days-schedule: Sets the opening and closing hours for all weekdays uniformly
        https://localhost:7037/days-schedule?openingHour={string}&closingHour={string}
        string openingHour: The opening hour in "HH:mm" format. e.g. "09:15"
        string closingHour: The closing hour in "HH:mm" format. e.g. "16:15"
            e.g. https://localhost:7037/days-schedule?openingHour=09%3A00&closingHour=16%3A00

    days: Sets the opening and closing hours for a specific day of the week
        https://localhost:7037/days?dayOfWeek={string}&openingHour={string}&closingHour={string}
        string dayOfWeek: The day of the week. e.g. "Monday", "Tuesday"
        string openingHour: The opening hour in "HH:mm" format. e.g. "09:15"
        string closingHour: The closing hour in "HH:mm" format. e.g. "16:15"
            e.g. https://localhost:7037/days?dayOfWeek=monday&openingHour=09%3A00&closingHour=19%3A00

    date: Sets the opening and closing hours for a specific date
        https://localhost:7037/date?date={string}&openingHour={string}&closingHour={string}
        string date The specific date in "yyyy-MM-dd" format. e.g. "2024-11-08"
        string openingHour: The opening hour in "HH:mm" format. e.g. "09:15"
        string closingHour: The closing hour in "HH:mm" format. e.g. "16:15"
            e.g. https://localhost:7037/date?date=2024-11-25&openingHour=09%3A00&closingHour=19%3A00

    days-close: Marks specific days of the week as closed
        https://localhost:7037/days-close?daysOfWeek={string}
        string dayOfWeek: A comma-separated list of days of the week. e.g., "Sunday, Wednesday"
            e.g. https://localhost:7037/days-close?daysOfWeek=Sunday%2C%20Wednesday

    dates-close: Marks specific dates as closed
        https://localhost:7037/dates-close?dates={string}
        string dates A comma-separated list of dates in "yyyy-MM-dd" format. e.g. "2024-11-29, 2024-12-05"
            e.g. https://localhost:7037/dates-close?dates=2024-11-29%2C%202024-12-05

    schedule-calculator: Calculates the estimated completion date and time for a service based on the given duration and start date
        https://localhost:7037/schedule-calculator?minutes={int}&date={string}
        int minutes: The duration of the service in minutes. e.g. "120"
        string date: The start date and time in "yyyy-MM-dd HH:mm" format. e.g. "2024-11-08 09:15"
            e.g. https://localhost:7037/dates-close?dates=2024-11-29%2C%202024-12-05


Additional libraries:

    LanguageExt.Core: It supports robust, consistent handling of data transformations and error management, aligning with principles from functional programming to create a safer, more predictable, and testable codebase
    Swashbuckle.AspNetCore: For its ease of setup, powerful features, and ability to generate reliable, interactive documentation quickly, which aligns with best practices in API development