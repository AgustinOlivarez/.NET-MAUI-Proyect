using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Services.Core
{
    public interface IAddPlayerService
    {
        Task<(bool Success, string Message)> SavePlayerAsync(Player player);
    }
}
