using System.Collections.Generic;

namespace Bifrost.Devices.Gpio.Abstractions
{
    public interface IGpioController
    {
        IGpioPin OpenPin(int pinNumber);

        IDictionary<string, string> Pins { get; }
    }
}