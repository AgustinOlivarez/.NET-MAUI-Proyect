using System.Threading.Tasks;

namespace MauiApp1.Services.Hardware
{
    public interface IPhotoService
    {
        Task<string?> CapturePhotoAsync();
    }
}
