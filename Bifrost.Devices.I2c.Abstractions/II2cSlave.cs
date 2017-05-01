using System;

namespace Bifrost.Devices.I2c.Abstractions
{
    public interface II2cSlave
    {
        byte GetI2cAddress();

        void Initialize();

        bool IsConnected();
    }
}
