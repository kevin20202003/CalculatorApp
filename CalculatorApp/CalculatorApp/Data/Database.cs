using CalculatorApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Operation>().Wait();
        }

        public Task<List<Operation>> GetOperationsAsync()
        {
            return _database.Table<Operation>().ToListAsync();
        }

        public Task<int> SaveOperationAsync(Operation operation)
        {
            return _database.InsertAsync(operation);
        }

        public Task<int> DeleteOperationAsync(Operation operation)
        {
            return _database.DeleteAsync(operation);
        }
    }
}
