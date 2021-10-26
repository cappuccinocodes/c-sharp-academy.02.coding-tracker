using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace CodeTracker1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Projects\Tutorials\CodeTracker\CodeTracker1\d4.sqlite";

            //bool dbExists = File.Exists(path);
            

            if (true)
                //Console.WriteLine("Database Exists");
                using (var connection = new SqliteConnection("Data Source=C:\\Projects\\Tutorials\\CodeTracker\\CodeTracker1\\d4.sqlite"))
                {
                   
                        connection.Open();
                        var tableCmd = connection.CreateCommand();
                        tableCmd.CommandText = "create table bolas (name varchar(20), score int) ";
                        tableCmd.ExecuteNonQuery();
                        connection.Close();
                 
                    
                    Console.WriteLine("Table Created");
                }
            else
                DatabaseManager.CreateDatabase(path);
            return;
        }
    }
}
