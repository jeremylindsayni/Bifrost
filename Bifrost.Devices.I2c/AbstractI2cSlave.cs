using Bifrost.Devices.I2c.Abstractions;
using System;
using System.Runtime.InteropServices;

namespace Bifrost.Devices.I2c
{
    public abstract class AbstractI2cSlave : II2cSlave
    {
        public I2cDevice Slave { get; set; }

        public abstract byte I2cAddress { get; }

        public void Initialize()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                throw new NotImplementedException();
            }
            else
            {
                int busId = I2cDevice.I2cBus;

                var i2cSettings = new I2cConnectionSettings(this.I2cAddress);

                var i2cDevice = new I2cDevice(busId, i2cSettings);

                this.Slave = i2cDevice;
            }
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}
