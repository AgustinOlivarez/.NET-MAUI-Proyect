using System;

namespace MauiApp1.Services.Hardware
{
    public interface IAccelerationService
    {
        void Start(Action onShake);
        void Stop();
    }
}
