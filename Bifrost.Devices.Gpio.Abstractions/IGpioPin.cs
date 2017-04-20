using Bifrost.Devices.Gpio.Core;
using System;

namespace Bifrost.Devices.Gpio.Abstractions
{
    public interface IGpioPin : IDisposable
    {
        void Write(GpioPinValue pinValue);

        void SetDriveMode(GpioPinDriveMode driveMode);
    }
}
