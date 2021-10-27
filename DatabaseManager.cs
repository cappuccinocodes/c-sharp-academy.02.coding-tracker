using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker1
{
    class DatabaseManager
    {
        public static void CreateDatabase()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"create table coding ('Id' INTEGER NOT NULL, 'Date' INTEGER, 'Duration' INTEGER, PRIMARY KEY('Id' AUTOINCREMENT)  ) ";
                tableCmd.ExecuteNonQuery();
                connection.Close();
                
                Console.WriteLine("Table Created");
            }
        }
    }
}
