using System;

namespace Bifrost.Devices.I2c.Abstractions
{
    public interface II2cDevice : IDisposable
    {
        string DeviceSelector { get; }

        II2cConnectionSettings ConnectionSettings { get; }

        string DeviceId { get; }

        byte[] ReadBytes(byte registerAddress); 

        byte[] ReadBytes(byte registerAddress, int size);

        IWord ReadWord(byte registerAddress);
    }
}
