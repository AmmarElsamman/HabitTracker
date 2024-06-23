using Npgsql;
using System.Configuration;
using System.Globalization;

namespace Habit_Tracker
{
    internal class DatabaseHandler
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HabitTracker"].ConnectionString;

        public DatabaseHandler()
        {
            this.CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    Console.Out.WriteLine("Opening connection");
                    conn.Open();

                    var unitTableCmd = conn.CreateCommand();
                    unitTableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS unit(
                            id SERIAL,
                            name TEXT UNIQUE,
                            PRIMARY KEY(id)
                            )";
                    unitTableCmd.ExecuteNonQuery();

                    var habitTableCmd = conn.CreateCommand();
                    habitTableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS habit(
                            id SERIAL,
                            name TEXT UNIQUE,
                            unit_id INTEGER REFERENCES unit(id) ON DELETE CASCADE,
                            PRIMARY KEY(id)
                            )";
                    habitTableCmd.ExecuteNonQuery();

                    var recordTableCmd = conn.CreateCommand();
                    recordTableCmd.CommandText =
                        @"CREATE Table IF NOT EXISTS record(
                            id SERIAL,
                            habit_id INTEGER REFERENCES habit(id) ON DELETE CASCADE,
                            date TEXT,
                            quantity INTEGER,
                            PRIMARY KEY(id)
                            )";
                    recordTableCmd.ExecuteNonQuery();

                    var indexTableCmd = conn.CreateCommand();
                    indexTableCmd.CommandText =
                        @"CREATE INDEX IF NOT EXISTS date_b ON record(date)";
                    indexTableCmd.ExecuteNonQuery();



                    conn.Close();


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

        }


        public void GetHabits()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var readCmd = conn.CreateCommand();

                readCmd.CommandText = $"SELECT habit.id,habit.name as habit ,unit.name as unit FROM habit JOIN unit ON habit.unit_id = unit.id";

                var reader = readCmd.ExecuteReader();

                Console.WriteLine("id   |            habit              |            unit      ");
                Console.WriteLine("-----+-------------------------------+----------------------");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string habit = reader.GetString(1);
                        string unit = reader.GetString(2);

                        Console.WriteLine($"{id}    -       {habit}      -      {unit}");
                        Console.WriteLine("-------------------------------------------\n");
                    }
                }
                else
                {
                    Console.WriteLine("Table is empty");
                }

                conn.Close();
            }
        }

        public void GetAllRecords()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var readCmd = conn.CreateCommand();

                readCmd.CommandText = $"select record.id as Id, record.date as Date, habit.name as Habit, record.quantity as Quantity,unit.name as Unit FROM record JOIN habit ON record.habit_id=habit.id JOIN unit ON habit.unit_id=unit.id";

                var reader = readCmd.ExecuteReader();

                Console.WriteLine("id   |date                 |   Habit          |   Quantity        |   Unit        ");
                Console.WriteLine("-----+------------------------+-------------------------+------------------+---------------------");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US"));
                        string habit = reader.GetString(2);
                        int quantity = reader.GetInt32(3);
                        string unit = reader.GetString(4);

                        Console.WriteLine($"{id}    -  {date.ToString("dd-MMM-yyyy")}        -  {habit}         -  {quantity}        -  {unit}        ");
                        Console.WriteLine("---------------------------------------------------------------------------------------------\n");
                    }
                }
                else
                {
                    Console.WriteLine("Table is empty");
                }

                conn.Close();
            }

        }

        public void InsertHabit(string habit, string unit)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var insertCmd = conn.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO habit(name,unit_id) VALUES ('{habit}',(SELECT id FROM unit WHERE name='{unit}'))";
                insertCmd.ExecuteNonQuery();

                conn.Close();

            }
        }

        public void DeleteHabit(int recordId)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var deleteCmd = conn.CreateCommand();

                deleteCmd.CommandText = $"DELETE FROM habit WHERE id='{recordId}'";
                deleteCmd.ExecuteNonQuery();

                conn.Close();
            }
        }


        public void InsertUnit(string unit)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var insertCmd = conn.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO unit(name) VALUES ('{unit}')";
                insertCmd.ExecuteNonQuery();

                conn.Close();

            }
        }


        // TODO: CHECK FOR DUPLICATES
        public void InsertRecord(int habit_id, string date, int quantity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var insertCmd = connection.CreateCommand();

                insertCmd.CommandText = $"INSERT INTO record(habit_id, date, quantity) VALUES ({habit_id}, '{date}', {quantity}) ON CONFLICT (habit_id,date) DO UPDATE SET quantity = record.quantity + {quantity}";

                insertCmd.ExecuteNonQuery();

                connection.Close();
            }
        }


        public void DeleteRecord(int recordId)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var deleteCmd = conn.CreateCommand();

                deleteCmd.CommandText = $"DELETE FROM record WHERE id='{recordId}'";
                deleteCmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void UpdateRecord(int recordId, string date, int quantity)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var updateCmd = conn.CreateCommand();
                updateCmd.CommandText = $"UPDATE habit1 SET date='{date}', quantity={quantity} WHERE id={recordId}";
                updateCmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void UpdateRecord(int recordId, string date)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var updateCmd = conn.CreateCommand();
                updateCmd.CommandText = $"UPDATE record SET date='{date}' WHERE id={recordId}";
                updateCmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void UpdateRecord(int recordId, int quantity)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var updateCmd = conn.CreateCommand();
                updateCmd.CommandText = $"UPDATE record SET quantity='{quantity}' WHERE id={recordId}";
                updateCmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public bool CheckIfRowExists(int recordId)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = $"(SELECT 1 FROM record WHERE id={recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    return false;
                }
                conn.Close();

            }

            return true;
        }

        public bool CheckIfHabitExists(string habit)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = $"SELECT 1 FROM habit WHERE name='{habit}'";

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    return false;
                }
                conn.Close();
            }

            return true;
        }

        public bool CheckIfHabitExists(int habit_id)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = $"SELECT 1 FROM habit WHERE id={habit_id}";

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    return false;
                }
                conn.Close();
            }

            return true;
        }

        public bool CheckIfunitExists(string unit)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = $"SELECT 1 FROM unit WHERE name='{unit}'";

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    return false;
                }
                conn.Close();
            }

            return true;
        }


    }
}
