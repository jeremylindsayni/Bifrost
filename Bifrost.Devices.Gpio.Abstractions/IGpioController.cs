using Bifrost.Devices.Gpio.Core;

namespace Bifrost.Devices.Gpio.Abstractions
{
    public interface IGpioController
    {
        IGpioPin OpenPin(int pinNumber);
    }
}