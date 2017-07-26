using System;

namespace Bifrost.Devices.I2c.Abstractions
{
    public interface IWord
    {
        byte MostSignificantByte { get; set; }

        byte LeastSignificantByte { get; set; }
    }
}
