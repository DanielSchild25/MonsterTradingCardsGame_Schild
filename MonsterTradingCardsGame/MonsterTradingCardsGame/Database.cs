using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterTradingCardsGame
{

    using Dict = Dictionary<string, object>;
    public class Database
    {

        readonly NpgsqlConnection db;
        static Database? instance;
        
        public static Database Base
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

      
        Database(string host = "localhost", string user = "postgres", string password = "", string database = "mtcg")
        {
            var db = new NpgsqlConnection($"Host={host}; Username={user};Password={password};Database={database}");

            db.Open();
            this.db = db;
            Console.WriteLine("Connect successful!");
        }

        public async Task<Dict?> Read(string toRead, string table, Dict? restrictions = null, bool random = false)
        {
            Dict result = new();
            string stringCommand = $"SELECT {toRead} FROM {table} ";
            
            if (restrictions != null)
            {
                string[] keys = restrictions.Keys.ToArray();

                if(restrictions.ContainsValue("NULL"))
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        keys[i] = $"{keys[i]} is NULL";
                    }
                }
                else
                {
                    for(int i = 0; i < keys.Length; i++)
                    {
                        keys[i] = $"{keys[i]}=@{keys[i]}";
                    }
                }
                

                if(restrictions.Count > 0)
                {
                    stringCommand += "WHERE " + string.Join(" AND ", keys);
                }
            }

            if (random)
                stringCommand += " ORDER BY RANDOM() LIMIT 1";

                using var sqlCommand = new NpgsqlCommand(stringCommand + ";");
            sqlCommand.Connection = db;

            if(restrictions != null)
            {
                foreach(string key in restrictions.Keys)
                {
                    sqlCommand.Parameters.AddWithValue(key, restrictions[key]);
                }
            }

            await using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                if (!reader.HasRows) return null;

                int index = 0;
                while(reader.Read())
                {
                    for(int i = 0;  i < reader.FieldCount; i++)
                    {
                        result[reader.GetName(i) + index] = reader.GetValue(i);
         
                    }
                    index++;
                }

            }
            return result;
        }

        public async Task<bool> Update(string table, Dict data, Dict restrictions)
        {
            if (data.Count == 0) 
                return false;

            //string stringCommand = $"UPDATE {table} SET {data.Keys.First()}=@{data.Keys.First()} ";
            string stringCommand = $"UPDATE {table} SET ";

            string[] parts = data.Keys.ToArray();

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = $"{parts[i]}=@{parts[i]}";
            }

            stringCommand += string.Join(", ", parts);

            string[] keys = restrictions.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = $"{keys[i]}=@{keys[i]}";
            }

            if (restrictions.Count > 0)
            {
                stringCommand += " WHERE " + string.Join(" AND ", keys);
            }

            using var sqlCommand = new NpgsqlCommand(stringCommand + ";");
            sqlCommand.Connection = db;

            foreach (string key in restrictions.Keys)
            {
                sqlCommand.Parameters.AddWithValue(key, restrictions[key]);
            }

            foreach (string key in data.Keys)
            {
                sqlCommand.Parameters.AddWithValue(key, data[key]);
            }

            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                Console.WriteLine(stringCommand);
            }
            catch (System.Data.Common.DbException ex)
            {
                return false;
            }
            return true;
        }



        public async Task<bool> Write(string table, Dict data)
        {
            if (data.Count == 0) 
                return false;

            string stringCommand = $"INSERT INTO {table} ({string.Join(", ", data.Keys)}) VALUES (@{string.Join(", @", data.Keys)});";
            using var sqlCommand = new NpgsqlCommand(stringCommand);
            sqlCommand.Connection = db;

            foreach(string key in data.Keys)
            {
                sqlCommand.Parameters.AddWithValue(key, data[key]);
            }

            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                Console.WriteLine(stringCommand);
            }
            catch (System.Data.Common.DbException ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(string table, Dict restrictions = null)
        {
            string stringCommand = $"DELETE FROM {table} ";

            if (restrictions != null)
            {
                string[] keys = restrictions.Keys.ToArray();

                if (restrictions.ContainsValue("NULL"))
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        keys[i] = $"{keys[i]} is NULL";
                    }
                }
                else
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        keys[i] = $"{keys[i]}=@{keys[i]}";
                    }
                }


                if (restrictions.Count > 0)
                {
                    stringCommand += "WHERE " + string.Join(" AND ", keys);
                }
            }

            using var sqlCommand = new NpgsqlCommand(stringCommand + ";");
            sqlCommand.Connection = db;

            if (restrictions != null)
            {
                foreach (string key in restrictions.Keys)
                {
                    sqlCommand.Parameters.AddWithValue(key, restrictions[key]);
                }
            }
            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                Console.WriteLine(stringCommand);
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
