using Bifröst.Devices.Gpio.Core;
using Bifröst.Devices.Gpio.Abstractions;
using System;
using System.IO;

namespace Bifröst.Devices.Gpio
{
    public class GpioPin : IGpioPin, IDisposable
    {
        public int PinNumber { get; private set; }

        public string GpioPath { get; private set; }

        public GpioPin(int pinNumber, string gpioPath)
        {
            this.PinNumber = pinNumber;
            this.GpioPath = gpioPath;
        }

        public void Dispose()
        {
            // equivalent to unexport
            // write a file with the pin name to the unexport folder
            // delete files and folders from the device path
            throw new NotImplementedException();
        }

        public void SetDriveMode(GpioPinDriveMode driveMode)
        {
            //var path = GpioController.DevicePath;
            // write in or out to the "direction" file for this pin.
            File.WriteAllText(Path.Combine(this.GpioPath, "direction"), "out");
            Directory.SetLastWriteTime(Path.Combine(this.GpioPath), DateTime.UtcNow);
        }

        public void Write(GpioPinValue pinValue)
        {
            //var path = GpioController.DevicePath;
            // write a 1 or a 0 to the "value" file for this pin
            File.WriteAllText(Path.Combine(this.GpioPath, "value"), ((int)pinValue).ToString());
            Directory.SetLastWriteTime(Path.Combine(this.GpioPath), DateTime.UtcNow);
        }
    }
}
