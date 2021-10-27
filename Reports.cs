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
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Type 0 to return to Main Menu.");
            Console.WriteLine("Type 1 to generate report by month");
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
                        Console.WriteLine("Invalid Command.");
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
            Console.WriteLine("Please type month number");
            int month = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please type year (format: YYYY)");
            int year = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM coding";

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
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();
            }
        }
    }
}
