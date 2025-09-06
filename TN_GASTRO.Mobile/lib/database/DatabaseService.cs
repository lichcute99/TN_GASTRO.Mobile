using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TN_GASTRO.Mobile.lib.database
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService(SQLiteAsyncConnection connection)
        {
            _db = connection;
        }

        public async Task<int> InsertAsync<T>(T entity) where T : new()
        {
            var result = await _db.InsertAsync(entity);
            Console.WriteLine($"Insert: {entity} -> result: {result}");
            return result;
        }

        public async Task<int> ExecuteAsync(string query, params object[] args)
        {
            var result = await _db.ExecuteAsync(query, args);
            Console.WriteLine($"Execute: {query} [{string.Join(",", args)}] -> {result}");
            return result;
        }

        public async Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new()
        {
            var result = await _db.QueryAsync<T>(query, args);
            Console.WriteLine($"Query: {query} -> {result.Count} rows");
            return result;
        }

        public async Task<List<Dictionary<string, object>>> QueryMapAsync(string query, params object[] args)
        {
            var result = await _db.QueryAsync<object>(query, args);
            var dictList = new List<Dictionary<string, object>>();

            foreach (var row in result)
            {
                var dict = new Dictionary<string, object>();
                foreach (var prop in row.GetType().GetProperties())
                {
                    dict[prop.Name] = prop.GetValue(row);
                }
                dictList.Add(dict);
            }

            return dictList;
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : new()
        {
            var result = await _db.UpdateAsync(entity);
            Console.WriteLine($"Update: {entity} -> result {result}");
            return result;
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : new()
        {
            var result = await _db.DeleteAsync(entity);
            Console.WriteLine($"Delete: {entity} -> result {result}");
            return result;
        }

        public async Task<int> DeleteAllAsync<T>() where T : new()
        {
            var result = await _db.DeleteAllAsync<T>();
            Console.WriteLine($"DeleteAll: {typeof(T).Name} -> result {result}");
            return result;
        }

        public async Task<int> CountAsync<T>() where T : new()
        {
            var count = await _db.Table<T>().CountAsync();
            Console.WriteLine($"Count: {typeof(T).Name} -> {count}");
            return count;
        }

        public async Task<T> GetLastAsync<T>() where T : new()
        {
            var item = await _db.Table<T>().OrderByDescending(x => x).FirstOrDefaultAsync();
            return item;
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            return await _db.ExecuteAsync(query);
        }
    }
}
