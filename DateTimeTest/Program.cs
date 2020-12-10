using System;

namespace DateTimeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt=Convert.ToDateTime("2018-09-18");
            DateTime startWeek = dt.AddDays(1 - int.Parse(dt.DayOfWeek.ToString("d")));
            Console.WriteLine("本周一："+startWeek);

            DateTime endWeek = startWeek.AddDays(6);
            Console.WriteLine("本周日："+endWeek);

            DateTime startMonth = dt.AddDays(1 - dt.Day);
            Console.WriteLine("本月初“"+startMonth);


            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            Console.WriteLine("本月末："+endMonth);
        }
    }
}
