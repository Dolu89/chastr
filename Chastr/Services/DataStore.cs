using Chastr.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Chastr.Services
{
    public class DataStore<T> : IDataStore<T> where T : IItem, new()
    {
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "chastr.db3");
        private readonly SQLiteAsyncConnection db;
        public DataStore()
        {
            db = new SQLiteAsyncConnection(dbPath);
            CreateTableAsync().GetAwaiter();
        }

        private async Task CreateTableAsync()
        {
            await db.CreateTableAsync<T>();
        }

        public async Task<bool> AddItemAsync(T item)
        {
            var rowsAdded = await db.InsertAsync(item);
            return await Task.FromResult(RowsAdded(rowsAdded));
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var rowsDeleted = await db.DeleteAsync(id);
            return await Task.FromResult(RowsAdded(rowsDeleted));
        }

        public async Task<T> GetItemAsync(string id)
        {
            return await db.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh)
        {
            return await db.Table<T>().ToListAsync();
        }

        public AsyncTableQuery<T> GetQueryableItemsAsync()
        {
            return db.Table<T>();
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            var rowsAdded = await db.UpdateAsync(item);
            return await Task.FromResult(RowsAdded(rowsAdded));
        }

        private bool RowsAdded(int rowsAdded)
        {
            if (rowsAdded == 0)
                return false;
            return true;
        }
    }
}
