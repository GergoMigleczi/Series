using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Series
{
    struct Series
    {
        public int year;
        public int month;
        public int day;
        public string title;
        public int season;
        public int episode;
        public int length;
        public int seenOrNot; //0=not seen 1=seen
        public Series(int year, int month, int day, string title, int season, int episode, int length, int seenOrNot)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.title = title;
            this.season = season;
            this.episode = episode;
            this.length = length;
            this.seenOrNot = seenOrNot;
        }

    }
    class SeriesClass
    {
        /*
            2018.01.19  Date of release
            Puzzles     Name of series
            3x10        season X episode
            43          length in minutes
            0           seen by list maker or not 0=not 1=yes
            NI          Release date is unkown
            Puzzles 
            3x11 
            43 
            0
         */
        static List<Series> listOfSeries = new List<Series>();

        //Task 1: read and store the data of the list.txt 
        static void Task1()
        {
            StreamReader sr = new StreamReader("lista.txt");
            while (!sr.EndOfStream)
            {
                int year;
                int month;
                int day;
                string date = sr.ReadLine();
                if (date == "NI")
                {

                    year = 0;
                    month = 0;
                    day = 0;
                }
                else
                {
                    year = int.Parse(date.Substring(0, 4));
                    month = int.Parse(date.Substring(5, 2));
                    day = int.Parse(date.Substring(8));
                }
                string title = sr.ReadLine();
                string[] se = sr.ReadLine().Split('x');
                int season = int.Parse(se[0]);
                int episode = int.Parse(se[1]);
                int length = int.Parse(sr.ReadLine());
                int seenOrNot = int.Parse(sr.ReadLine());

                Series item = new Series(year, month, day, title, season, episode, length, seenOrNot);
                listOfSeries.Add(item);
            }
            sr.Close();

        }

        //Task 2: Print the number of records that contain information of the release date
        static void Task2()
        {
            Console.WriteLine("Task 2");
            int counter = 0;
            foreach (Series item in listOfSeries)
            {
                if (item.year != 0)
                {
                    counter++;
                }
            }
            Console.WriteLine($"\tThere are {counter} records containing information of the release date.");
        }

        //Task 3: Calculate what percentage of the episodes are seen by the maker of the list
        static void Task3()
        {
            Console.WriteLine("Task 3");
            int seen = 0;
            foreach (Series item in listOfSeries)
            {
                if (item.seenOrNot == 1) //1=seen, 0=not seen
                {
                    seen++;
                }
            }
            Console.WriteLine($"The maker of the list saw the {Math.Round(((double)seen / listOfSeries.Count) * 100, 2)}% of the episodes.");
        }

        //Task 4: Print how much time the maker spent on series watching
        static void Task4()
        {
            Console.WriteLine("Task 4");
            int time = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;
            foreach (Series item in listOfSeries)
            {
                if (item.seenOrNot == 1)
                {
                    time += item.length;
                }
            }
            day = time / 60 / 24;
            hour = (time - day * 60 * 24) / 60;
            minute = (time - day * 60 * 24) % 60;
            Console.WriteLine($"\tThey spent {day} days {hour} hours and {minute} minutes on watching series. ");

        }

        //Task 5: Ask the user for a date in the form of yyyy.mm.dd
        //        Print which episodes the makes has not seen, which were released before that date
        static void Task5()
        {
            Console.WriteLine("Task 5");
            Console.Write("\tDate (yyyy.mm.dd)= ");
            string[] date = Console.ReadLine().Split('.');
            int year = int.Parse(date[0]);
            int month = int.Parse(date[1]);
            int day = int.Parse(date[2]);

            foreach (Series item in listOfSeries)
            {
                if (item.seenOrNot == 0 && item.year != 0 && item.month != 0 && item.day != 0 && item.year <= year && item.month <= month && item.day <= day)
                {
                    Console.WriteLine($"\t{item.season}x{item.episode}\t{item.title}");
                }
            }
        }

        //Task 6: Make a method that takes a date as an argument and returns which day that date was
        static string DayOfTheWeek(int year, int month, int day)
        {
            string[] days = { "m", "tu", "w", "th", "f", "sa", "su" };
            int[] months = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            if (month < 3)
                year = year - 1;
            return days[(year + year / 4 - year / 100 + year / 400 + months[month - 1] + day) % 7];
        }

        //Task 7: Ask the user for a day of the week. Print what episodes get released on that day of the weak
        static void Task7()
        {
            Console.WriteLine("Task 7");
            Console.Write("\tDay in a short form (m, tu, w, th, f ,sa, su)= ");
            string day = Console.ReadLine();
            List<string> series = new List<string>();
            foreach (Series item in listOfSeries)
            {

                if (item.year != 0 && DayOfTheWeek(item.year, item.month, item.day) == day)
                {
                    if (!series.Contains(item.title))
                    {
                        series.Add(item.title);
                    }
                }
            }
            if (series.Count > 0)
            {
                foreach (string item in series)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("No series get released on that day of the week.");
            }
        }

        //Task 8: Calculate the all together length of each series and the number of episodes
        //        List the result into the a txt
        static void Task8()
        {
            StreamWriter sw = new StreamWriter("sum.txt");

            int episodes = 0;
            int totalLength = 0;

            List<string> series = new List<string>();
            foreach (Series item in listOfSeries)
            {
                if (!series.Contains(item.title))
                {
                    series.Add(item.title);
                }
            }
            foreach (string i in series)
            {
                episodes = 0;
                totalLength = 0;
                foreach (Series item in listOfSeries)
                {
                    if (i == item.title)
                    {
                        episodes++;
                        totalLength += item.length;
                    }
                }
                sw.WriteLine($"{i} {totalLength} {episodes}");
            }

            sw.Flush();
            sw.Close();
        }
        //1=seen, 0=not seen
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Console.WriteLine();
            Task3();
            Console.WriteLine();
            Task4();
            Console.WriteLine();
            Task5();
            Console.WriteLine();
            Task7();
            Task8();
            Console.ReadKey();
        }
    }
}
