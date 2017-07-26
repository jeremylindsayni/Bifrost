using Bifrost.Devices.I2c.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Bifrost.Devices.I2c
{
    public class I2cDevice : II2cDevice
    {
        private int OPEN_READ_WRITE = 2;

        private int I2C_SLAVE = 0x0703;

        [DllImport("libc.so.6", EntryPoint = "open")]
        public static extern int Open(string fileName, int mode);

        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        private extern static int Ioctl(int fd, int request, int data);

        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        internal static extern IntPtr Read(int handle, byte[] data, int length);

        [DllImport("libc.so.6", EntryPoint = "close", SetLastError = true)]
        public static extern int Close(int busHandle);

        public I2cDevice(string busId, II2cConnectionSettings connectionSettings)
        {
            this.ConnectionSettings = connectionSettings;
            this.BusId = busId;
        }

        public string BusId { get; private set; }

        public static string I2cBus
        {
            get
            {
                if (IsWindows)
                {
                    throw new NotImplementedException();
                    // write a file and wait for a response
                }
                else
                { 
                    var directory = new DirectoryInfo("/dev/");
                    var i2cFiles = directory.GetFiles().Where(m => m.Name.StartsWith("i2c-")).ToArray();

                    if (i2cFiles.Count() == 1)
                    {
                        return i2cFiles[0].Name;
                    }

                    if (i2cFiles.Count() > 1)
                    {
                        throw new ArgumentOutOfRangeException("More than one I2C bus was found. Please specify the bus directly in the I2cDevice constructor.");
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("No I2C bus was found. Please check that there is an I2C bus for this device - perhaps use 'i2cdetect -l' from a terminal, or the Bifröst Cartographer.");
                    }
                }
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

        public byte[] ReadBytes(byte registerAddress, int startAddress, int size)
        {
            if (IsWindows)
            {
                throw new NotImplementedException();
                // write a file and wait for a response
            }
            else
            {
                // Get the I2C bus name
                int i2cBushandle = GetI2cBusHandle();

                ControlI2cDevice(registerAddress, i2cBushandle);

                return ReadFromDevice(i2cBushandle, size);
            }
        }

        public byte[] ReadBytes(byte registerAddress, int size)
        {
            return this.ReadBytes(registerAddress, 0, size);
        }

        public IWord ReadWord(byte registerAddress)
        {
            return this.ReadWord(registerAddress, 0);
        }

        public IWord ReadWord(byte registerAddress, int startAddress)
        {
            var word = this.ReadBytes(registerAddress, startAddress, 2);

            return new Word { MostSignificantByte = word[0], LeastSignificantByte = word[1] };
        }

        private static bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        private byte[] ReadFromDevice(int i2cBushandle, int numberOfBytesToRead)
        {
            var deviceDataInMemory = new byte[numberOfBytesToRead];

            var deviceReadReturnCode = Read(i2cBushandle, deviceDataInMemory, deviceDataInMemory.Length);

            return deviceDataInMemory;
        }

        private void ControlI2cDevice(byte registerAddress, int i2cBushandle)
        {
            // Now allow control of the remote device
            var deviceReturnCode = Ioctl(i2cBushandle, I2C_SLAVE, registerAddress);

            if (deviceReturnCode < 0)
            {
                throw new Exception("Could not establish a connection to the I2C device.");
            }
        }

        private int GetI2cBusHandle()
        {
            var i2cBushandle = Open(this.BusId, OPEN_READ_WRITE);

            if (i2cBushandle < 0)
            {
                throw new Exception("No I2C bus was found. Please check that there is an I2C bus for this device - perhaps use 'i2cdetect -l' from a terminal, or the Bifröst Cartographer.");
            }

            return i2cBushandle;
        }
    }
}