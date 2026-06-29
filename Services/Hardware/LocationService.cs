using System;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;

namespace MauiApp1.Services.Hardware
{
    public class LocationService : ILocationService
    {
        public async Task<(double? Latitude, double? Longitude, string? Message)> GetLocationAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                    return (null, null, "Permiso de ubicación denegado.");

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);
                if (location == null)
                    return (null, null, "No se pudo obtener la ubicación.");

                return (location.Latitude, location.Longitude, null);
            }
            catch (Exception ex)
            {
                return (null, null, $"Error de ubicación: {ex.Message}");
            }
        }
    }
}
