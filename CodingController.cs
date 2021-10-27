using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CodeTracker1
{
    internal static class CodingController
    {
        static readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        internal static void GetUserCommand()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Records.");
            Console.WriteLine("Type 3 to Delete Records.");
            Console.WriteLine("Type 4 to Update Records.");
            Console.WriteLine("Type 5 to Generate Reports.");
            try
            {
                int command = Convert.ToInt32(Console.ReadLine());
                switch (command)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Get();
                        break;
                    case 2:
                        Post();
                        break;
                    case 3:
                        Delete();
                        break;
                    case 4:
                        Update();
                        break;
                    case 5:
                        Reports.GetReportCommand();
                        break;
                    default:
                        Console.WriteLine("Invalid Command.");
                        GetUserCommand();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        internal static void Post()
        {
            long date = GetDateInput();
            long duration = GetDurationInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ({date}, {duration})";
                tableCmd.ExecuteNonQuery();
                connection.Close();

                Console.WriteLine("Table Created");
            }
            Console.WriteLine($"Your time was logged: date({date}), duration({duration}).");

            GetUserCommand();
        }

        internal static void Delete()
        {
            Console.WriteLine("Please type Id of the record would like to delete");
            string inputId = Console.ReadLine();
            var Id = Int32.Parse(inputId);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from coding WHERE Id = '{Id}'";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine($"Record with Id {Id} was deleted.");
            GetUserCommand();
        }

        internal static void Get()
        {
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

                GetUserCommand();
            }
        }

        internal static void Update()
        {
            Console.WriteLine("Please type Id of the record would like to update");
            string inputId = Console.ReadLine();
            var Id = Int32.Parse(inputId);
            long date = GetDateInput();
            long duration = GetDurationInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE coding SET date = {date}, duration = {duration} WHERE Id = {Id}";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"Your time was logged: date({date}), duration({duration}).");

            GetUserCommand();
        }

        internal static long GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy)");
            string dateInput = Console.ReadLine();
            try
            {
                var parsedDate = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"));

                long date = parsedDate.Ticks;
                return date;
            }
            catch (Exception e)
            {
                string error = e.Message;
                Console.WriteLine("Not a valid date.");
                GetDateInput();
                throw;
            }
        }

        internal static long GetDurationInput()
        {
            Console.WriteLine("Please insert the duration: (Format: hh:mm)");
            string dateInput = Console.ReadLine();
            try
            {
                var parsedDuration = TimeSpan.Parse(dateInput);
                long date = parsedDuration.Ticks;
                return date;
            }
            catch (Exception e)
            {
                string error = e.Message;
                Console.WriteLine("Not a valid date.");
                GetDurationInput();
                throw;
            }
        }
    }
}