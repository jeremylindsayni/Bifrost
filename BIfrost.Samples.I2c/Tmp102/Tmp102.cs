using Bifrost.Devices.I2c;
using System;

namespace Tmp102
{
    public class Tmp102 : AbstractI2cSlave
    {
        private byte I2C_ADDRESS;

        public Tmp102(A0PinConnection pinConnection)
        {
            I2C_ADDRESS = (byte)pinConnection;
        }

        public override byte I2cAddress
        {
            get
            {
                return I2C_ADDRESS;
            }
        }

        public float GetTemperature(Scale scale)
        {
            var temperatureWord = this.Slave.ReadWord(I2C_ADDRESS);

            // this formula is from the data sheet.
            // 1. Using logical OR, add the most significant bit (multiplied by 256) and least significant bytes
            // 2. Right shift the sum by 4 places (i.e. divide by 16)
            // 3. Multiply by 0.0625
            var bytesAddedTogether = temperatureWord.MostSignificantByte << 8 | temperatureWord.LeastSignificantByte;

            var bytesRightShiftedByFourBits = bytesAddedTogether >> 4;

            if (scale == Scale.Celcius)
            {
                return bytesRightShiftedByFourBits * 0.0625f;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}