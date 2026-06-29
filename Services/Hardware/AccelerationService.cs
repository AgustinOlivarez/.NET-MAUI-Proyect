using System;
using Microsoft.Maui.Devices.Sensors;

namespace MauiApp1.Services.Hardware
{
    public class AccelerationService : IAccelerationService
    {
        private Action? _onShake;
        private const double ShakeThreshold = 2.5;
        private DateTime _lastShake = DateTime.MinValue;

        public void Start(Action onShake)
        {
            _onShake = onShake;
            try
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Start(SensorSpeed.UI);
            }
            catch
            {
                // ignore
            }
        }

        public void Stop()
        {
            try
            {
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                Accelerometer.Stop();
            }
            catch
            {
                // ignore
            }
            _onShake = null;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var x = e.Reading.Acceleration.X;
            var y = e.Reading.Acceleration.Y;
            var z = e.Reading.Acceleration.Z;

            var magnitude = Math.Sqrt(x * x + y * y + z * z);
            if (magnitude > ShakeThreshold)
            {
                if ((DateTime.UtcNow - _lastShake).TotalMilliseconds < 1000) return;
                _lastShake = DateTime.UtcNow;
                _onShake?.Invoke();
            }
        }
    }
}
