using Microsoft.Extensions.Logging;
using MauiApp1.Repositories;
using System.IO;
using Microsoft.Maui.Storage;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif           
            builder.Services.AddTransient<ViewModels.PlayersViewModel>();
            builder.Services.AddTransient<Views.PlayersPage>();

            builder.Services.AddTransient<ViewModels.PlayerDetailViewModel>();
            builder.Services.AddTransient<Views.PlayerDetailPage>();

            // Registrar AddPlayer
            builder.Services.AddTransient<ViewModels.AddPlayerViewModel>();
            builder.Services.AddTransient<Views.AddPlayerPage>();

            // AddPlayer service
            builder.Services.AddSingleton<Services.Core.IAddPlayerService, Services.Core.AddPlayerService>();

            // Hardware services
            builder.Services.AddSingleton<Services.Hardware.IPhotoService, Services.Hardware.PhotoService>();
            builder.Services.AddSingleton<Services.Hardware.ILocationService, Services.Hardware.LocationService>();
            builder.Services.AddSingleton<Services.Hardware.IAccelerationService, Services.Hardware.AccelerationService>();
            builder.Services.AddSingleton<Services.Hardware.IVibrationService, Services.Hardware.VibrationService>();

            // Registrar repositorio SQLite como Singleton
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "players.db3");
            builder.Services.AddSingleton<IPlayerRepository>(sp => new PlayerRepositorySQLite(dbPath));

            return builder.Build();
        }
    }
}
