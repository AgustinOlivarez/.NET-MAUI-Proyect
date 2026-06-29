using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Repositories
{
    public interface IPlayerRepository
    {
        Task InitializeAsync();
        Task<int> AddAsync(Player player);
        Task<List<Player>> GetAllAsync();
        Task<Player> GetByIdAsync(int id);
        Task<int> UpdateAsync(Player player);
        Task<int> DeleteAsync(Player player);
    }
}
