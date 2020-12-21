using System;

namespace TimeCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            var assignmentMinutes1 = 245;
            var assignmentDate1 = new DateTime(2020, 12, 21).AddHours(14);
            Console.WriteLine($"Start Date: {assignmentDate1}. Minutes: {assignmentMinutes1}");
            var scheduler = new Scheduler();
            var endDate1 = scheduler.GetEndDate(assignmentDate1, assignmentMinutes1);
            Console.WriteLine($"End Date: {endDate1}");

            var assignmentMinutes2 = 9000;
            var assignmentDate2 = new DateTime(2020, 12, 28).AddHours(12);
            Console.WriteLine($"Start Date: {assignmentDate2}. Minutes: {assignmentMinutes2}"); 
            var endDate2 = scheduler.GetEndDate(assignmentDate2, assignmentMinutes2);
            Console.WriteLine($"End Date: {endDate2}");

            Console.ReadLine();


        }
    }
}