using System;

namespace Bifrost.Devices.I2c.Abstractions
{
    public interface II2cSlave
    {
        byte I2cAddress { get; }

        void Initialize();

        bool IsConnected();
    }
}
