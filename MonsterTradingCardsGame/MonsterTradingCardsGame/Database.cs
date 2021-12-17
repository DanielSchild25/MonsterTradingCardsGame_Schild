using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterTradingCardsGame
{

    using Data = Dictionary<string, object>;
    class Database
    {

        readonly NpgsqlConnection db;
        static Database? instance;
        
        public static Database self
        {
            get
            {
                if(instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }

        Database(string host = "localhost", string user = "dev", string password = "12345", string database = "mtcg")
        {
            var db = new NpgsqlConnection($"Host={host};Username={user};Password={password};Database={database}");
            db.Open();
            this.db = db;
        }

        public async Task<Data?> Read(string scope, string table, Data? parameters = null)
        {
            Data result = new();
            string scommand = $"SELECT {scope} FROM {table}";

            if(parameters != null)
            {
                string[] keys = parameters.Keys.ToArray();

                for(int i = 0; i < keys.Length; i++)
                {
                    keys[i] = $"{keys[i]}=@{keys[i]}";
                }

                if(parameters.Count > 0)
                {
                    scommand += "WHERE " + string.Join(" AND ", keys);
                }
            }

            using var command = new NpgsqlCommand(scommand + ";");
            command.Connection = db;

            if(parameters != null)
            {
                foreach(string key in parameters.Keys)
                {
                    command.Parameters.AddWithValue(key, parameters[key]);
                }
            }

            await using (var reader = await command.ExecuteReaderAsync())
            {
                if (!reader.HasRows) return null;
                reader.Read();

                for(int i = 0; i < reader.FieldCount; i++)
                {
                    result[reader.GetName(i)] = reader.GetValue(i);
                }
            }
            return result;
        }

        public async Task<bool> Write(string table, Data parameters)
        {
            if (parameters.Count == 0) return false;

            string scommand = $"INSERT INTO {table} ({string.Join(", ", parameters.Keys)}) VALUES (@{string.Join(", @", parameters.Keys)});";
            using var command = new NpgsqlCommand(scommand);
            command.Connection = db;

            foreach(string key in parameters.Keys)
            {
                command.Parameters.AddWithValue(key, parameters[key]);
            }

            try
            {
                await command.ExecuteNonQueryAsync();
                Console.WriteLine(scommand);
            }
            catch (System.Data.Common.DbException ex)
            {
                return false;
            }
            return true;
        }

        ~Database()
        {
            db.Close();
        }



    }
}
