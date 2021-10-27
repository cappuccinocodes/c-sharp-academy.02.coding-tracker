using System;
using System.Configuration;
using System.Globalization;
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
            Console.WriteLine("Type 1 to Insert Data.");
            try
            {
                int command = Convert.ToInt32(Console.ReadLine());
                switch (command)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        InsertCodingData();
                        break;
                    default:
                        Console.WriteLine("Invalid Command.");
                        InsertCodingData();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        internal static void InsertCodingData()
        {
            long date = GetDateInput();
            long duration = GetDurationInput();
            PersistData(date, duration);
           
         
               

                
                //using (var connection = new SqliteConnection(connectionString))
                //{
                //    connection.Open();
                //    var tableCmd = connection.CreateCommand();
                //    tableCmd.CommandText = $"INSERT INTO coding VALUES ()";
                //    tableCmd.ExecuteNonQuery();
                //    connection.Close();

                //    Console.WriteLine("Table Created");
                //}
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

        internal static void PersistData(long date, long duration)
        {
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
        }
    }
}