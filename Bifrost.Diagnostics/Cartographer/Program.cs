using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Bifrost.Diagnostics.Cartographer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("|  THE CARTOGRAPHER  |");
            Console.WriteLine("----------------------");

            // Ip address?
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                foreach (var ua in ni.GetIPProperties()
                                        .UnicastAddresses
                                        .Where(m => m.Address.AddressFamily == AddressFamily.InterNetwork)
                                        .Where(m => m.Address.ToString() != "127.0.0.1")) {
                        //Console.WriteLine($"IP Address: {ua.Address.ToString()}");
                        FormattedWriteLine("IP Address:", ua.Address.ToString());
                }
            }

            // MAC address?
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up))
            {
                //Console.WriteLine($"MAC Address: {nic.GetPhysicalAddress()}");
                FormattedWriteLine("MAC Address:", nic.GetPhysicalAddress().ToString());
            }
            
            // Processor type
            var chipArchitecture = RuntimeInformation.OSArchitecture;
            FormattedWriteLine("Chip Architecture:", chipArchitecture.ToString());
            
            // Operating System
            var osNameAndVersion = RuntimeInformation.OSDescription;
            FormattedWriteLine("Operating System:",osNameAndVersion);

            // .net framework
            var frameworkDescription = RuntimeInformation.FrameworkDescription;
            FormattedWriteLine("Framework version:", frameworkDescription);

            // machine name
            var machineName = Dns.GetHostName();
            FormattedWriteLine("Machine name:", machineName);
            
            // What is the operating system?
            if (IsWindows)
            {
                // logged in as:
                var currentUser = Environment.GetEnvironmentVariable("USERNAME");
                Console.WriteLine($"Current User: {currentUser}");

                Console.WriteLine("Windows operating system");
                // Is there a Bifrost UWP app installed?
                var localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
                Console.WriteLine(localAppData);
                // Is the Bifrost UWP application running?
            }
            else 
            {
                var currentUser = Environment.GetEnvironmentVariable("USER");
                FormattedWriteLine("Current User:", currentUser);
            }

            if (IsWindows)
            {

            }
            else 
            {
                // Are there I2c buses?
                // What are the names of the buses?
                var directory = new DirectoryInfo("/dev/");
                var i2cFiles = directory.GetFiles().Where(m => m.Name.StartsWith("i2c-")).ToArray();

                foreach(var i2cFile in i2cFiles)
                {
                    FormattedWriteLine("I2C Bus Name:", i2cFile.Name);
                }

                // Are there I2c devices?
                // What are the device addresses, and potentially identify them?
                var p = new Process()
                {
                    EnableRaisingEvents = false
                };
                
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "i2cdetect";
                p.StartInfo.Arguments = "-y 1";
                
                p.Start();
                
                string data = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                var d = data.Split(Environment.NewLine);
                foreach (var row in d)
                {
                    if (row.Contains(":")) 
                    {
                        var r = row.Split(":")[1].Replace("--", "").Trim();

                        if (r != String.Empty)
                        {
                            var n = r.Split(" ");
                            foreach(var q in n) {
                                FormattedWriteLine("I2C Device found at:", q);
                            }
                        }
                    }
                }
            }
            
            if (IsWindows)
            {

            }
            else 
            {
                // Are there Gpio devices set up?
                var isSetupForGpio = Directory.Exists("/sys/class/gpio");
                Console.WriteLine("Set up for GPIO at: /sys/class/gpio");

                // What pins and statuses?
                var gpioDirectoryInfo = new DirectoryInfo("/sys/class/gpio");
                var gpioPins = gpioDirectoryInfo.GetDirectories().Where(m => m.Name.StartsWith("gpio") && !m.Name.StartsWith("gpiochip"));
                Console.WriteLine($"Number of GPIO pins active: {gpioPins.Count()}");
                foreach(var pin in gpioPins)
                {
                    Console.WriteLine($"Pin {pin.Name}");
                }
            }
                
        }

        private static bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        private static void FormattedWriteLine(string leftText, string rightText) 
        {
            leftText = leftText.PadRight(30);
            rightText = rightText.PadRight(30);

            Console.WriteLine($"| {leftText} | {rightText} |");
        }
    }
}
