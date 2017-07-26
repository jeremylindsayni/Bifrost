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
        
        public IDictionary<string, string> Pins
        {
            get
            {
                Console.WriteLine($"About to start reading pins from {DevicePath}");
                DirectoryInfo gpioPins = new DirectoryInfo(DevicePath);

                IDictionary<string, string> pinNameValues = new Dictionary<string, string>();

                var pinNames = gpioPins.GetDirectories()
                    .Where(m => m.Name.StartsWith("gpio"))
                    .Where(m => !m.Name.StartsWith("gpiochip"))
                    .Select(m => m.Name)
                    .ToArray();

                Console.WriteLine($"Number of pin folders = {pinNames.Count()}");

                foreach (var pinName in pinNames)
                {
                    Console.WriteLine($"About to add value for {Path.Combine(DevicePath, pinName, "value")}");
                    pinNameValues.Add(pinName, File.ReadAllText(Path.Combine(DevicePath, pinName, "value")));
                }

                return pinNameValues;
            }
        }

        public IGpioPin OpenPin(int pinNumber)
        {
            if (pinNumber < 1 || pinNumber > 26)
            {
                throw new ArgumentOutOfRangeException("Valid pins are between 1 and 26.");
            }
            // add a file to the export directory with the name <<pin number>>
            // add folder under device path for "gpio<<pinNumber>>"
            var gpioDirectoryPath = Path.Combine(DevicePath, string.Concat("gpio", pinNumber.ToString()));

            var gpioExportPath = Path.Combine(DevicePath, "export");
            
            if (!Directory.Exists(gpioDirectoryPath))
            {
                Debug.WriteLine("Directory doesn't exist - exporting");
                File.WriteAllText(gpioExportPath, pinNumber.ToString());
                Debug.WriteLine("Creating the directory");
                Directory.CreateDirectory(gpioDirectoryPath);
            }

            // instantiate the gpiopin object to return with the pin number.
            return new GpioPin(pinNumber, gpioDirectoryPath);
        }
    }
}
