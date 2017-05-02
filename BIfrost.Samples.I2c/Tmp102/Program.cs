using Bifrost.Devices.I2c;
using System;

namespace Tmp102
{
    class Program
    {
        static void Main(string[] args)
        {
            var temperatureSensor = new Tmp102(A0PinConnection.Ground);

            temperatureSensor.Initialize();

            var temperature = temperatureSensor.GetTemperature();

            Console.WriteLine(temperature);
        }
    }

    enum A0PinConnection
    {
        Ground = 0x48,
        VCC = 0x49,
        SDA = 0x4A,
        SCL = 0x4B
    }

    class Tmp102 : AbstractI2cSlave
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

        public float GetTemperature()
        {
            var temperatureWord = this.Slave.ReadWord(I2C_ADDRESS);

            // this formula is from the data sheet.
            // 1. Add the most significant and least significant bytes (using logical OR)
            // 2. Right shift the sum by 4 places (i.e. divide by 16)
            // 3. Multiply by 0.0625
            var bytesAddedTogether = temperatureWord.MostSignificantByte << 8 | temperatureWord.LeastSignificantByte;

            var bytesRightShiftedByFourBits = bytesAddedTogether >> 4;

            return bytesRightShiftedByFourBits * 0.0625f;
        }
    }
}
