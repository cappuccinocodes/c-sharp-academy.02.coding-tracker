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
            Console.WriteLine("\n\nWhat would you like to do?");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Records.");
            Console.WriteLine("Type 3 to Delete Records.");
            Console.WriteLine("Type 4 to Update Records.");
            Console.WriteLine("Type 5 to Generate Reports.\n\n");
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
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                        GetUserCommand();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                GetUserCommand();
            }
        }

        internal static void Post()
        {
            long date = GetDateInput();
            long duration = GetDurationInput();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ({date}, {duration})";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch
            {
                Console.WriteLine("\nPlease enter date with correct format.\n");
                GetUserCommand();
            }

            string inputDate = new DateTime(date).ToString("dd-MM-yy");
            string inputDuration = new DateTime(duration).ToString("HH:mm");

            Console.WriteLine($"\n\nYour time was logged. Date: {inputDate}; Duration: {inputDuration}.\n\n");

            GetUserCommand();
        }

        internal static void Delete()
        {
            Console.WriteLine("\n\nPlease type Id of the record would like to delete.\n\n");
            string inputId = Console.ReadLine();
            var Id = Int32.Parse(inputId);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from coding WHERE Id = '{Id}'";
                int rowCount = tableCmd.ExecuteNonQuery();

                string message =
                    rowCount == 0 ? 
                    $"\n\nRecord with Id {Id} doesn't exist.\n\n" : 
                    $"\n\nRecord with Id {Id} was deleted.\n\n";

                Console.WriteLine(message); 
               
            }
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

                Console.WriteLine("\n\n");

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();
                Console.WriteLine("\n\n");

                GetUserCommand();
            }
        }

        internal static void Update()
        {
            Console.WriteLine("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

            string inputId = Console.ReadLine();

            if (inputId == "0")
            {
                GetUserCommand();
            }

            var Id = Int32.Parse(inputId);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding WHERE Id = {Id})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (checkQuery == 0)
                { 
                    Console.WriteLine($"\n\nRecord with Id {Id} doesn't exist.\n\n");
                    GetUserCommand();
                }

                long date = GetDateInput();
                long duration = GetDurationInput();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE coding SET date = {date}, duration = {duration} WHERE Id = {Id}";
                tableCmd.ExecuteNonQuery();

                string inputDate = new DateTime(date).ToString("dd-MM-yy");
                string inputDuration = new DateTime(duration).ToString("HH:mm");

                Console.WriteLine($"\n\nYour time was logged: date({inputDate}), duration({inputDuration}).\n\n");
                connection.Close();
            }

            GetUserCommand();
        }

        internal static long GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");
            DateTime result;

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                GetUserCommand();
            }

            bool success = DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out result);

            if (success)
            {
                var parsedDate = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"));
                long date = parsedDate.Ticks;
                return date;
            }

            Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
            return GetDateInput();
        }

        internal static long GetDurationInput()
        {
            Console.WriteLine("\n\nPlease insert the duration: (Format: hh:mm). Type 0 to return to main manu.\n\n");
            TimeSpan timeSpan;

            string durationInput = Console.ReadLine();
            if (durationInput == "0")
            {
                GetUserCommand();
            }

            bool success = TimeSpan.TryParseExact(durationInput, "h\\:mm", CultureInfo.InvariantCulture, out timeSpan);

            if (success)
            {
                var parsedDuration = TimeSpan.Parse(durationInput);
                long date = parsedDuration.Ticks;
                if (date < 0)
                {
                    Console.WriteLine("\n\nNegative Time Not allowed.\n\n");
                    GetDurationInput();
                }
                return date;
            }

            Console.WriteLine("\n\nNot a valid time.\n\n");
            return GetDurationInput();
        }
    }
}