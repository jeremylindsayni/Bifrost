using Bifrost.Devices.I2c.Abstractions;

namespace Bifrost.Devices.I2c
{
    public class I2cConnectionSettings : II2cConnectionSettings
    {
        public I2cConnectionSettings(int slaveAddress)
        {
            this.SlaveAddress = slaveAddress;
        }

        public int SlaveAddress { get; set; }
    }
}
