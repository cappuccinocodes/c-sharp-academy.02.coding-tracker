using System;
using System.Collections.Generic;
using System.Configuration;
using ConsoleTableExt;
using Microsoft.Data.Sqlite;

namespace CodeTracker1
{
    internal static class Reports
    {
        static readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public static void GetReportCommand()
        {
            Console.WriteLine("\n\nWhat would you like to do?");
            Console.WriteLine("Type 0 to return to Main Menu.");
            Console.WriteLine("Type 1 to generate report by month.\n\n");


            string commandInput = Console.ReadLine();

            if (string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("\nInvalid Command. Please choose an option\n");
                GetReportCommand();
            }

            int command = Convert.ToInt32(commandInput);

            switch (command)
            {
                case 0:
                    CodingController.GetUserCommand();
                    break;
                case 1:
                    ReportByMonth();
                    break;
                default:
                    Console.WriteLine("\n\nInvalid Command.\n\n");
                    GetReportCommand();
                    break;
            }
        }

        internal static void ReportByMonth()
        {
            int month = GetMonthInput();
            int year = GetYearInput();

            long start = HelperMethods.CalculateMonth(year, month).start;
            long end = HelperMethods.CalculateMonth(year, month).end;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM coding WHERE Date BETWEEN {start} AND {end}";

                List<Coding> tableData = new List<Coding>();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new Coding
                                {
                                    Id = reader.GetInt32(0),
                                    Date = new DateTime(reader.GetInt64(1)).ToShortDateString(),
                                    Duration = new TimeSpan(reader.GetInt64(2)).ToString()
                                });
                        }
                }
                else
                {
                    Console.WriteLine("\n\nNo rows found.\n\n");
                    CodingController.GetUserCommand();
                }
                reader.Close();

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();

                CodingController.GetUserCommand();
            }
        }

        private static int GetYearInput()
        {
            Console.WriteLine($"\n\nPlease type year number.\n\n");

            string yearInput = Console.ReadLine();

            while (string.IsNullOrEmpty(yearInput))
            {
                Console.WriteLine($"\n\nInput can't be empty.\n\n");
                yearInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(yearInput)) break;
            }

            int year = Convert.ToInt32(yearInput);

            while (!(year > 1970 && year < 2099))
            {
                Console.WriteLine("\n\nInvalid Year. Please try again.\n\n");
                year = Convert.ToInt32(Console.ReadLine());

                if (year > 1970 && year < 2099)
                    break;
            }
            return year;
        }

        private static int GetMonthInput()
        {
            Console.WriteLine($"\n\nPlease type month number.\n\n");

            string monthInput = Console.ReadLine();

            while (string.IsNullOrEmpty(monthInput))
            {
                Console.WriteLine($"\n\nInput can't be empty.\n\n");
                monthInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(monthInput)) break;
            }

            int month = Convert.ToInt32(monthInput);

            while (!(month > 0 && month < 13))
            {
                Console.WriteLine("\n\nInvalid Month. Please try Again.\n\n");
                month = Convert.ToInt32(Console.ReadLine());

                if (month > 0 && month < 13)
                    break;
            }

            return month;
        }
    }
}
