using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services.Core;
using MauiApp1.Services.Hardware;

namespace MauiApp1.ViewModels
{
    public partial class AddPlayerViewModel : ObservableObject
    {
        private readonly IAddPlayerService _addPlayerService;
        private readonly IPhotoService _photoService;
        private readonly ILocationService _locationService;
        private readonly IAccelerationService _accelerationService;
        private readonly IVibrationService _vibrationService;

        public AddPlayerViewModel(IAddPlayerService addPlayerService,
            IPhotoService photoService,
            ILocationService locationService,
            IAccelerationService accelerationService,
            IVibrationService vibrationService)
        {
            _addPlayerService = addPlayerService ?? throw new ArgumentNullException(nameof(addPlayerService));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _accelerationService = accelerationService ?? throw new ArgumentNullException(nameof(accelerationService));
            _vibrationService = vibrationService ?? throw new ArgumentNullException(nameof(vibrationService));
        }

        [ObservableProperty]
        string nombre;

        [ObservableProperty]
        string posicion;

        [ObservableProperty]
        string equipo;

        [ObservableProperty]
        string imagePath;

        [ObservableProperty]
        double? latitude;

        [ObservableProperty]
        double? longitude;

        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string statusMessage;

        [RelayCommand]
        public async Task CapturePhotoAsync()
        {
            try
            {
                var path = await _photoService.CapturePhotoAsync();
                if (!string.IsNullOrEmpty(path))
                {
                    ImagePath = path;
                    StatusMessage = "Foto capturada.";
                }
                else
                {
                    StatusMessage = "No se pudo capturar la foto.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al capturar foto: {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task GetLocationAsync()
        {
            try
            {
                var (lat, lon, msg) = await _locationService.GetLocationAsync();
                if (!string.IsNullOrEmpty(msg))
                {
                    StatusMessage = msg;
                    return;
                }

                Latitude = lat;
                Longitude = lon;
                StatusMessage = "Ubicación obtenida.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error de ubicación: {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var player = new Player
                {
                    Nombre = Nombre,
                    Posicion = Posicion,
                    Equipo = Equipo,
                    ImagePath = ImagePath ?? string.Empty,
                    // Agregamos '?? 0.0' para convertir el double? (nulo) a double (no nulo)
                    Latitude = Latitude ?? 0.0,
                    Longitude = Longitude ?? 0.0
                };

                var (success, message) = await _addPlayerService.SavePlayerAsync(player);
                StatusMessage = message;

                if (success)
                {
                    try { _vibrationService.Vibrate(TimeSpan.FromMilliseconds(150)); } catch { }
                    await ClearAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public Task ClearAsync()
        {
            Nombre = string.Empty;
            Posicion = string.Empty;
            Equipo = string.Empty;
            ImagePath = string.Empty;
            Latitude = null;
            Longitude = null;
            StatusMessage = string.Empty;
            return Task.CompletedTask;
        }

        public void StartListeningShake()
        {
            try
            {
                _accelerationService.Start(async () =>
                {
                    await ClearAsync();
                    StatusMessage = "Formulario limpiado por 'shake'.";
                });
            }
            catch { }
        }

        public void StopListeningShake()
        {
            try { _accelerationService.Stop(); } catch { }
        }
    }
}
