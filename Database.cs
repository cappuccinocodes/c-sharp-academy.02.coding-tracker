using System.Configuration;
using System.Data.SQLite;
using Coding_Tracker.models;
using System.Globalization;

namespace Coding_Tracker
{
    internal class Database
    {
        internal readonly string? connectionString = ConfigurationManager.AppSettings.Get("dbConnectionString");
        internal void Initialize()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_habits (
                                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        start_date TEXT,
                                        end_date TEXT
                                        )";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal void Insert(CodingHabit habit)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"INSERT INTO coding_habits (
                                        start_date, end_date)
                                        VALUES(
                                        '{habit.StartDate}', '{habit.EndDate}')";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal bool Update(CodingHabit habit)
        {
            bool updated = true;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_habits WHERE Id='{habit.Id}')";
                int lineExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if(lineExists == 0)
                {
                    updated = false;
                }
                else
                {
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = @$"UPDATE coding_habits
                                              SET start_date='{habit.StartDate}', end_date='{habit.EndDate}'
                                              WHERE id='{habit.Id}'";
                    tableCmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            return updated;
        }

        // better way of handling the data returned from the DB when there is one line?
        internal List<CodingHabit> SelectOne(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM coding_habits WHERE Id='{id}' LIMIT 1";

                List<CodingHabit> tableData = new();

                SQLiteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        tableData.Add(
                        new CodingHabit
                        {
                            Id = reader.GetInt32(0),
                            StartDate = DateTime.ParseExact(reader.GetString(1), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")),
                            EndDate = DateTime.ParseExact(reader.GetString(2), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")),
                        });
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        internal List<CodingHabit> SelectAll()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM coding_habits";

                List<CodingHabit> tableData = new();

                SQLiteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new CodingHabit
                        {
                            Id = reader.GetInt32(0),
                            StartDate = DateTime.ParseExact(reader.GetString(1), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")),
                            EndDate = DateTime.ParseExact(reader.GetString(2), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")),
                        });
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        internal int Delete(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"DELETE FROM coding_habits
                                        WHERE Id='{id}'";
                int rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();

                return rowCount;
            }
        }
    }
}
