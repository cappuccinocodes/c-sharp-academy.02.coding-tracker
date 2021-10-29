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
            try
            {
                int command = Convert.ToInt32(Console.ReadLine());
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
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        internal static void ReportByMonth()
        {
            while (true)
            {
                Console.WriteLine($"\n\nPlease type month number.\n\n");
                int month = Convert.ToInt32(Console.ReadLine());
                if (!(month > 0 && month < 13))
                {
                    Console.WriteLine("\n\nInvalid Month. Please try Again.\n\n");
                    return;
                } else
                {
                    break;
                }
            }


            Console.WriteLine("\n\nPlease type year (format: YYYY).\n\n");
            int year = Convert.ToInt32(Console.ReadLine());
            if (!(year > 1970 && year < 2099))
            {
                Console.WriteLine("\n\nInvalid Year. Please try again.\n\n");
                year = Convert.ToInt32(Console.ReadLine());
            }

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
                    try
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
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CodingController.GetUserCommand();
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
    }
}
