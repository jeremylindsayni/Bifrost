using System;
using System.Diagnostics;
using Bifrost.Devices.I2c.Abstractions;
using System.Globalization;

namespace Bifrost.Devices.I2c
{
    public class I2cDevice : II2cDevice
    {
        private Process process;

        public I2cDevice(int busId, II2cConnectionSettings connectionSettings)
        {
            this.ConnectionSettings = connectionSettings;
            this.BusId = busId;
        }

        public int BusId { get; private set; }

        public static int I2cBus
        {
            get
            {
                var p = new Process()
                {
                    EnableRaisingEvents = false
                };

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "i2cdetect";
                p.StartInfo.Arguments = "-l";

                p.Start();

                string data = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                
                var fullI2cBus = data.SplitOnSpaceOrTabs()[0];

                return Convert.ToInt16(fullI2cBus.Replace("i2c-", string.Empty));
            }
        }
        
        public II2cConnectionSettings ConnectionSettings { get; private set; }

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

        public byte[] ReadBytes(byte registerAddress, int size)
        {
            return this.ReadBytes(registerAddress, 0, size);
        }

        public byte[] ReadBytes(byte registerAddress, int startAddress, int size)
        {
            throw new NotImplementedException();
        }

        public IWord ReadWord(byte registerAddress)
        {
            return this.ReadWord(registerAddress, 0);
        }

        public IWord ReadWord(byte registerAddress, int startAddress)
        {
            process = new Process()
            {
                EnableRaisingEvents = false
            };

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "i2cget";
            process.StartInfo.Arguments = $"-y {BusId} {ConnectionSettings.SlaveAddress} {startAddress} w";

            process.Start();

            string data = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            var lsbString = data.Substring(2, 2);
            int lsb = Int32.Parse(lsbString, NumberStyles.AllowHexSpecifier);

            // Get MSB & parse as integer
            var msbString = data.Substring(4, 2);
            int msb = Int32.Parse(msbString, NumberStyles.AllowHexSpecifier);

            // Shift bits as indicated in TMP102 docs & return
            return new Word { MostSignificantByte = (byte)msb, LeastSignificantByte = (byte)lsb };
        }
    }
}
