using System;

namespace Bifrost.Devices.I2c.Abstractions
{
    public interface II2cDevice : IDisposable
    {
        II2cConnectionSettings ConnectionSettings { get; }

        string BusId { get; }

        string DeviceId { get; }

        byte[] ReadBytes(byte registerAddress, int startAddress, int length); 

        byte[] ReadBytes(byte registerAddress, int length);

        IWord ReadWord(byte registerAddress, int startAddress);

        IWord ReadWord(byte registerAddress);
    }
}
