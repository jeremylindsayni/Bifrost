using System;
using Bifrost.Devices.I2c.Abstractions;

namespace Bifrost.Devices.I2c
{
    public class Word : IWord
    {
        public byte MostSignificantByte { get; set; }
        public byte LeastSignificantByte { get; set; }
    }
}
