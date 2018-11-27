using System.Collections.Generic;

namespace Bifrost.Devices.Gpio.Abstractions
{
    public interface IGpioController
    {
        IGpioPin OpenPin(int pinNumber);

        void ClosePin(int pinNumber);

        IDictionary<int, IGpioPin> Pins { get; }
    }
}
