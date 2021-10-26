using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker1
{
    class DatabaseManager
    {
        public static void CreateDatabase(string path)
        {
            File.Create("C:\\Projects\\Tutorials\\CodeTracker\\CodeTracker1\\db4.sqlite");
            Console.WriteLine("Database Created");

            using (var connection = new SqliteConnection("Data Source=db4.sqlite"))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "CREATE TABLE Coding ('Id' INTEGER,'Date'INTEGER 'Duration'  INTEGER); ";
                tableCmd.ExecuteNonQuery();
                Console.WriteLine("Table Created");
            }
        }
    }
}
