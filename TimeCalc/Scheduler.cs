using System;
using System.Collections.Generic;
using System.Text;

namespace TimeCalc
{
    public class Scheduler
    {
        public DateTime GetEndDate(DateTime assignmentDate, int minutes)
        {
            var holidaysCalendar = new HolidaysCalendar(assignmentDate);
            assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holidaysCalendar);
            assignmentDate = ValidateAndMoveWorkingHours(assignmentDate, holidaysCalendar);
            var assignmentHours = minutes / 60;

            const int dayHour = 8;
            for (; assignmentHours >= dayHour;)
            {
                assignmentHours -= dayHour;
                assignmentDate = assignmentDate.AddHours(24);
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holidaysCalendar);
            }

            var remainderHours = assignmentHours % dayHour;
            var remainderMinutes = minutes - (assignmentHours * 60);

            assignmentDate = assignmentDate.AddHours(remainderHours);
            assignmentDate = assignmentDate.AddMinutes(remainderMinutes);

            assignmentDate = MoveLastDayAfterHours(assignmentDate, holidaysCalendar);

            return assignmentDate;
        }

        private static DateTime ValidateDateAndPostponeTillTomorrow(DateTime assignmentDate, HolidaysCalendar holiday)
        {
            if (assignmentDate.DayOfWeek == DayOfWeek.Saturday || assignmentDate.DayOfWeek == DayOfWeek.Sunday || holiday.IsHoliday(assignmentDate))
            {
                Console.WriteLine($"Date: {assignmentDate} is a holiday or weekend.");
                assignmentDate = assignmentDate.AddDays(1);
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holiday);
            }
            return assignmentDate;
        }

        private static DateTime ValidateAndMoveWorkingHours(DateTime assignmentDate, HolidaysCalendar holiday)
        {
            const int workingStartTime = 8;
            const int workingEndTime = 17;
            if (assignmentDate.Hour >= workingEndTime)
            {
                Console.WriteLine($"Date: {assignmentDate} is after working hours.");
                assignmentDate = assignmentDate.AddDays(1);
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, assignmentDate.Day, workingStartTime, 0, 0);
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holiday);
            }
            else if (assignmentDate.Hour < workingStartTime)
            {
                Console.WriteLine($"Date: {assignmentDate} is before working hours.");
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, assignmentDate.Day, workingStartTime, 0, 0);
            }
            assignmentDate = ValidateLunchHours(assignmentDate);
            return assignmentDate;
        }

        private static DateTime ValidateLunchHours(DateTime assignmentDate)
        {
            if (assignmentDate.Hour >= 12 && assignmentDate.Hour <= 13)
            {
                Console.WriteLine($"Date: {assignmentDate} is a lunch time.");
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, assignmentDate.Day, assignmentDate.Hour + 1, assignmentDate.Minute, 0);
            }
            return assignmentDate;
        }

        private static DateTime MoveLastDayAfterHours(DateTime assignmentDate, HolidaysCalendar holiday)
        {
            if (assignmentDate.Hour > 17)
            {
                var nextDayHours = 8 + (assignmentDate.Hour - 17);
                var nextDay = assignmentDate.AddDays(1).Day;
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, nextDay, nextDayHours, assignmentDate.Minute, 0); //assignmentDate.AddDays(1).AddHours(nextDayHours);
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holiday);
                assignmentDate = ValidateLunchHours(assignmentDate);
            }

            assignmentDate = ValidateLunchHours(assignmentDate);
            return assignmentDate;
        }
    }
}
