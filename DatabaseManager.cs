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
            using (var connection = new SqliteConnection(path))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "create table coding (Id INT, date INT, duration INT) ";
                tableCmd.ExecuteNonQuery();
                connection.Close();
                
                Console.WriteLine("Table Created");
            }
        }
    }
}
