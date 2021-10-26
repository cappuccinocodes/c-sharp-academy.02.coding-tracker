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
            string databasePath = ConfigurationManager.AppSettings.Get("DatabasePathEB");
            Console.WriteLine("Hi there! I'm checking if database exists");
            bool dbExists = File.Exists(databasePath);
            
            if (!dbExists)
            {
                Console.WriteLine("Database doesn't exist, creating one...");
                DatabaseManager.CreateDatabase(databasePath);
            }
            else
                Console.WriteLine("Ready for next task");
            return;
        }
    }
}
