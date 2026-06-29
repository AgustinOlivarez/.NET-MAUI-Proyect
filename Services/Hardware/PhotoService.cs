using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;

namespace MauiApp1.Services.Hardware
{
    public class PhotoService : IPhotoService
    {
        public async Task<string?> CapturePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null) return null;

                var newFile = Path.Combine(FileSystem.AppDataDirectory, $"player_{Guid.NewGuid()}.jpg");
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }

                return newFile;
            }
            catch
            {
                return null;
            }
        }
    }
}
