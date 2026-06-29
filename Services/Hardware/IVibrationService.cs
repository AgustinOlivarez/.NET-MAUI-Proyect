using System;

namespace MauiApp1.Services.Hardware
{
    public interface IVibrationService
    {
        void Vibrate(TimeSpan duration);
    }
}
