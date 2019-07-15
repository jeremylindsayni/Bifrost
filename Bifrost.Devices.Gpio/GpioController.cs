using System;
using Bifrost.Devices.Gpio.Abstractions;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Bifrost.Devices.Gpio
{
    public class GpioController : IGpioController
    {
        private static GpioController instance = new GpioController();

        public static string DevicePath { get; private set; }

        public static IGpioController Instance
        {
            get
            {
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                if (isWindows) 
                {
                    try
                    {
                        var localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");

                        localAppData = localAppData.Replace("Administrator", "DefaultAccount");

                        localAppData = Path.Combine(localAppData, "Packages");

                        var piFolders = new DirectoryInfo(localAppData);

                        var biFrostFolder = piFolders.GetDirectories().Single(m => m.Name.StartsWith("Bifrost"));

                        var localStateDirectory = biFrostFolder.GetDirectories().Single(m => m.Name.StartsWith("LocalState"));

                        DevicePath = localStateDirectory.FullName;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Could not find device path for Windows application", ex);
                    }
                }
                else 
                {
                    DevicePath = @"/sys/class/gpio";
                }

                // need to check that the device path exists
                if (string.IsNullOrEmpty(DevicePath))
                {
                    throw new NullReferenceException("There is no value for the GPIO Directory.");
                }

                return instance;
            }
        }

        /// Return a pin object for all existing pins
        public IDictionary<int, IGpioPin> Pins
        {
            get
            {
                return new DirectoryInfo(DevicePath).GetDirectories()
                    .Where(di => di.Name.StartsWith("gpio"))
                    .Select(di => Tuple.Create(di, int.TryParse(di.Name.Substring("gpio".Length), out int value) ? value : (int?)null))
                    .Where(diAndPinNo => diAndPinNo.Item2.HasValue)
                    .ToDictionary(diAndPinNo => diAndPinNo.Item2.Value, diAndPinNo => (IGpioPin)new GpioPin(diAndPinNo.Item2.Value, diAndPinNo.Item1.FullName));
            }
        }

        public IGpioPin OpenPin(int pinNumber)
        {
            if (pinNumber < 1 || pinNumber > 26)
            {
                throw new ArgumentOutOfRangeException("Valid pins are between 1 and 26.");
            }
            // Create folder under device path for "gpio<<pinNumber>>"
            string gpioDirectoryPath = Path.Combine(DevicePath, string.Concat("gpio", pinNumber.ToString()));
            if (!Directory.Exists(gpioDirectoryPath))
            {
                // Write the pin number to the export device
                File.WriteAllText(Path.Combine(DevicePath, "export"), pinNumber.ToString());
                Directory.CreateDirectory(gpioDirectoryPath);
            }

            // Return a GpioPin object with the pin number
            return new GpioPin(pinNumber, gpioDirectoryPath);
        }

        public void ClosePin(int pinNumber)
        {
            if (pinNumber < 1 || pinNumber > 26)
            {
                throw new ArgumentOutOfRangeException("Valid pins are between 1 and 26.");
            }
            string gpioDirectoryPath = Path.Combine(DevicePath, string.Concat("gpio", pinNumber.ToString()));
            if (Directory.Exists(gpioDirectoryPath))
            {
                // Write the pin number to the unexport device
                File.WriteAllText(Path.Combine(DevicePath, "unexport"), pinNumber.ToString());
            }
        }
    }
}
