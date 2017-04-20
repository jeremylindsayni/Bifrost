using Bifröst.Devices.Gpio.Core;
using System;

namespace Bifröst.Devices.Gpio.Abstractions
{
    public interface IGpioPin : IDisposable
    {
        void Write(GpioPinValue pinValue);

        void SetDriveMode(GpioPinDriveMode driveMode);
    }
}
