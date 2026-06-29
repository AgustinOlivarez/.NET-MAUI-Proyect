using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MauiApp1.Models;
using SQLite;

namespace MauiApp1.Repositories
{
    public class PlayerRepositorySQLite : IPlayerRepository
    {
        private readonly SQLiteAsyncConnection _db;
        private bool _initialized = false;

        public PlayerRepositorySQLite(string dbPath)
        {
            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentNullException(nameof(dbPath));

            var dir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            await _db.CreateTableAsync<Player>();
            _initialized = true;
        }

        public async Task<int> AddAsync(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            await InitializeAsync();
            return await _db.InsertAsync(player);
        }

        public async Task<List<Player>> GetAllAsync()
        {
            await InitializeAsync();
            var list = await _db.Table<Player>().ToListAsync();
            return list;
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            await InitializeAsync();
            return await _db.FindAsync<Player>(id);
        }

        public async Task<int> UpdateAsync(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            await InitializeAsync();
            return await _db.UpdateAsync(player);
        }

        public async Task<int> DeleteAsync(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            await InitializeAsync();
            return await _db.DeleteAsync(player);
        }
    }
}
