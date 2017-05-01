using System;
using Bifrost.Devices.I2c.Abstractions;

namespace Bifrost.Devices.I2c
{
    public class I2CDevice : II2cDevice
    {
        public string DeviceSelector
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public II2cConnectionSettings ConnectionSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string DeviceId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte[] ReadBytes(byte registerAddress)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBytes(byte registerAddress, int size)
        {
            throw new NotImplementedException();
        }

        public IWord ReadWord(byte registerAddress)
        {
            throw new NotImplementedException();
        }
    }
}
