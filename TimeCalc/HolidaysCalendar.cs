using System;
using System.Linq;

namespace TimeCalc
{
    public class HolidaysCalendar
    {
        private DateTime NewYearsDate { get; set; }
        private DateTime FrthOfJuly { get; set; }
        private DateTime MemorialDay { get; set; }
        private DateTime LaborDay { get; set; }
        private DateTime ThanksGiving { get; set; }
        private int Year { get; set; }

        public HolidaysCalendar(DateTime date)
        {
            InitHolidays(date);
        }

        public bool IsHoliday(DateTime date)
        {
            if (Year != date.Year)
            {
                InitHolidays(date);
            }
            if (date.DayOfYear == NewYearsDate.DayOfYear || date.DayOfYear == FrthOfJuly.DayOfYear ||
                date.DayOfYear == MemorialDay.DayOfYear || date.DayOfYear == LaborDay.DayOfYear ||
                date.DayOfYear == ThanksGiving.DayOfYear)
            {
                Console.WriteLine($"Date: {date} is a holiday");
                return true;
            }
            return false;
        }

        private void InitHolidays(DateTime date)
        {
            Year = date.Year;
            NewYearsDate = new DateTime(date.Year, 1, 1);
            FrthOfJuly = new DateTime(date.Year, 7, 4);
            MemorialDay = GetMemorialDay(date.Year);
            LaborDay = NthDayOfMonth(date.Year, 9, DayOfWeek.Monday, 1);
            ThanksGiving = NthDayOfMonth(date.Year, 11, DayOfWeek.Thursday, 4);
        }

        private DateTime NthDayOfMonth(int year, int month, DayOfWeek dow, int n)
        {
            var days = DateTime.DaysInMonth(year, month);
            var nthday = (from day in Enumerable.Range(1, days)
                          let dt = new DateTime(year, month, day)
                          where dt.DayOfWeek == dow && (day - 1) / 7 == (n - 1)
                          select dt).FirstOrDefault();
            return nthday;
        }

        private DateTime GetMemorialDay(int year)
        {
            var dt = new DateTime(year, 5, 31);
            while (dt.DayOfWeek != DayOfWeek.Monday)
            {
                dt = dt.AddDays(-1);
            }
            return dt;
        }
    }
}