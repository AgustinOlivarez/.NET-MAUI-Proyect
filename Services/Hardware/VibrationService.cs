using System;
using Microsoft.Maui.Devices;

namespace MauiApp1.Services.Hardware
{
    public class VibrationService : IVibrationService
    {
        public void Vibrate(TimeSpan duration)
        {
            try
            {
                Vibration.Default.Vibrate(duration);
            }
            catch
            {
                // Ignore on platforms that don't support it
            }
        }
    }
}
