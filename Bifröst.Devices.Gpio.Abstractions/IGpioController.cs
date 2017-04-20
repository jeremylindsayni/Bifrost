using Bifröst.Devices.Gpio.Core;

namespace Bifröst.Devices.Gpio.Abstractions
{
    public interface IGpioController
    {
        IGpioPin OpenPin(int pinNumber);
    }
}