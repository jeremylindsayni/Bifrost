using Bifröst.Devices.Gpio;
using Bifröst.Devices.Gpio.Core;
using System;
using System.Diagnostics;

namespace GpioSwitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting...");
            
            if (args.Length != 2)
            {
                throw new ArgumentNullException("You must specify a pin and a logic level");
            }

            int pinNumber = ValidatePin(args[0]);
            int logicLevel = ValidateLogicLevel(args[1]);
            
            // create gpio controller
            Debug.WriteLine("About to instantiate the switch controller");
            var controller = GpioController.Instance;

            // open pin
            Debug.WriteLine("Opening pin " + pinNumber);
            var pin = controller.OpenPin(pinNumber);

            // set direction
            Debug.WriteLine("Setting the direction to out");
            pin.SetDriveMode(GpioPinDriveMode.Output);

            // set value
            if (logicLevel == 1)
            {
                Debug.WriteLine("Setting the value to high");
                pin.Write(GpioPinValue.High);
            }
            else
            {
                Debug.WriteLine("Setting the value to low");
                pin.Write(GpioPinValue.Low);
            }  
        }

        private static int ValidateLogicLevel(string argLogicLevel)
        {
            if (argLogicLevel != "1" && argLogicLevel != "0")
            {
                throw new ArgumentOutOfRangeException("Unknown logic level");
            }

            return Convert.ToInt16(argLogicLevel);
        }

        private static int ValidatePin(string argPinNumber)
        {
            int pinNumber;

            try
            {
                pinNumber = Int32.Parse(argPinNumber);

                if (pinNumber < 1 || pinNumber > 26)
                {
                    throw new ArgumentOutOfRangeException("Unknown value for pin");
                }
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException("Unknown value for pin");
            }
            
            return pinNumber;
        }
    }
}