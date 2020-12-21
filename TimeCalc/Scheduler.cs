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
            //Validating start date
            assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holidaysCalendar);
            //Validating if it's within working hours
            assignmentDate = ValidateAndMoveWorkingHours(assignmentDate, holidaysCalendar);
            //Calculating hours from minutes
            var assignmentHours = minutes / 60;

            //Looping through calculated hours 
            //If more or equals to 8 hours we will add full day(24 hours),
            //but will subtract 8 hours from calculated hours to take in consideration only 1 working day, which is 8 hours
            const int dayHour = 8;
            while (assignmentHours >= dayHour)
            {
                assignmentHours -= dayHour;
                assignmentDate = assignmentDate.AddHours(24);
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holidaysCalendar);
            }

            var remainderHours = assignmentHours % dayHour;
            var remainderMinutes = minutes - (assignmentHours * 60);

            assignmentDate = assignmentDate.AddHours(remainderHours);
            assignmentDate = assignmentDate.AddMinutes(remainderMinutes);

            assignmentDate = MoveToNextDayIfAfterHours(assignmentDate, holidaysCalendar);

            return assignmentDate;
        }

        /// <summary>
        /// Method validates whether given date is a holiday or a weekend
        /// Will call itself again if needed 
        /// </summary>
        /// <param name="assignmentDate"></param>
        /// <param name="holiday"></param>
        /// <returns></returns>
        private static DateTime ValidateDateAndPostponeTillTomorrow(DateTime assignmentDate, HolidaysCalendar holiday)
        {
            //Checking whether if it's a weekend or holiday and if yes, adds 1 day 
            //Recursively calls itself again if condition is true
            if (assignmentDate.DayOfWeek == DayOfWeek.Saturday || assignmentDate.DayOfWeek == DayOfWeek.Sunday || holiday.IsHoliday(assignmentDate))
            {
                Console.WriteLine($"Date: {assignmentDate} is a holiday or weekend.");
                assignmentDate = assignmentDate.AddDays(1);
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holiday);
            }
            return assignmentDate;
        }

        /// <summary>
        /// Method validates working hours from 8 am till 5 pm
        /// Will also vadidate lunch from 12 pm to 1 pm
        /// </summary>
        /// <param name="assignmentDate"></param>
        /// <param name="holiday"></param>
        /// <returns></returns>
        private static DateTime ValidateAndMoveWorkingHours(DateTime assignmentDate, HolidaysCalendar holiday)
        {
            const int workingStartTime = 8;
            const int workingEndTime = 17;
            if (assignmentDate.Hour >= workingEndTime)
            {
                Console.WriteLine($"Date: {assignmentDate} is after working hours.");
                assignmentDate = assignmentDate.AddDays(1);
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, assignmentDate.Day, workingStartTime, 0, 0);
                //Need to make sure that the next day is not a weekend/holiday
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holiday);
            }
            else if (assignmentDate.Hour < workingStartTime)
            {
                Console.WriteLine($"Date: {assignmentDate} is before working hours.");
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, assignmentDate.Day, workingStartTime, 0, 0);
            }
            //Validate for lunch hours
            assignmentDate = ValidateLunchHours(assignmentDate);
            return assignmentDate;
        }

        /// <summary>
        /// Validates given time for lunch hours between 12 pm and 1 pm 
        /// </summary>
        /// <param name="assignmentDate"></param>
        /// <returns></returns>
        private static DateTime ValidateLunchHours(DateTime assignmentDate)
        {
            if (assignmentDate.Hour >= 12 && assignmentDate.Hour <= 13)
            {
                Console.WriteLine($"Date: {assignmentDate} is a lunch time.");
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, assignmentDate.Day, assignmentDate.Hour + 1, assignmentDate.Minute, 0);
            }
            return assignmentDate;
        }

        private static DateTime MoveToNextDayIfAfterHours(DateTime assignmentDate, HolidaysCalendar holiday)
        {
            //Checks if after working hours
            if (assignmentDate.Hour >= 17)
            {
                //Calcultes the remainder of hours to append to the next day 
                var nextDayHours = 8 + (assignmentDate.Hour - 17);
                var nextDay = assignmentDate.AddDays(1).Day;
                assignmentDate = new DateTime(assignmentDate.Year, assignmentDate.Month, nextDay, nextDayHours, assignmentDate.Minute, 0); //assignmentDate.AddDays(1).AddHours(nextDayHours);
                //Validate if next day is not weekend/holiday
                assignmentDate = ValidateDateAndPostponeTillTomorrow(assignmentDate, holiday);
                //Validate lunch hours
                assignmentDate = ValidateLunchHours(assignmentDate);
            }

            assignmentDate = ValidateLunchHours(assignmentDate);
            return assignmentDate;
        }
    }
}
