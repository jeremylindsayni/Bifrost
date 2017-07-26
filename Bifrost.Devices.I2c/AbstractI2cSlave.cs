using Bifrost.Devices.I2c.Abstractions;
using System;

namespace Bifrost.Devices.I2c
{
    public abstract class AbstractI2cSlave : II2cSlave
    {
        public I2cDevice Slave { get; set; }

        public abstract byte I2cAddress { get; }
        
        public void Initialize()
        {
            var busId = I2cDevice.I2cBus;

            var i2cSettings = new I2cConnectionSettings(this.I2cAddress);

            this.Slave = new I2cDevice(busId, i2cSettings);
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}