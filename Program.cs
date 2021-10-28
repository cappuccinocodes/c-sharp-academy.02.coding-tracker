using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace CodeTracker1
{
    class Program
    {
        static void Main(string[] args)
        {
            string databasePath = ConfigurationManager.AppSettings.Get("DatabasePath");
            Console.WriteLine("Hi there! I'm checking if database exists\n\n");
            bool dbExists = File.Exists(databasePath);
            
            if (!dbExists)
            {
                Console.WriteLine("Database doesn't exist, creating one...\n\n");
                DatabaseManager.CreateDatabase();
            }
            else
            {
                Console.WriteLine("Database exists...\n\n");
                CodingController.GetUserCommand();
            }
        }
    }
}
