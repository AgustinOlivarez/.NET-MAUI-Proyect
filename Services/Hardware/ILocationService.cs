using System.Threading.Tasks;

namespace MauiApp1.Services.Hardware
{
    public interface ILocationService
    {
        Task<(double? Latitude, double? Longitude, string? Message)> GetLocationAsync();
    }
}
