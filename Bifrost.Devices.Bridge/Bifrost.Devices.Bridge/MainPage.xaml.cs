using Bifrost.DictionaryComparer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bifrost.Devices.Bridge
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Dictionary<string, string> oldGpioValues;

        private Comparer dictionaryComparer = new Comparer();

        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private GpioController gpioController = GpioController.GetDefault();

        private IDictionary<int, GpioPin> pins = new Dictionary<int, GpioPin>();

        private GpioPin pin;

        public MainPage()
        {
            this.InitializeComponent();

            oldGpioValues = dictionaryComparer.GetFilesRecursively(localFolder.Path) as Dictionary<string, string>;

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await MonitorStorageFolder();

            InitializePins();
        }

        private void InitializePins()
        {
            foreach (var filePath in oldGpioValues.Keys)
            {
                var pin = GetPin(filePath);

                UpdatePins(pin, filePath);
            }
        }

        private async Task MonitorStorageFolder()
        {
            var query = localFolder.CreateFileQueryWithOptions(new QueryOptions { });

            query.ContentsChanged += Query_ContentsChanged;

            var files = await query.GetFilesAsync();
        }

        private void Query_ContentsChanged(IStorageQueryResultBase sender, object args)
        {
            var newGpioValues = dictionaryComparer.GetFilesRecursively(localFolder.Path) as Dictionary<string, string>;

            var differences = dictionaryComparer.GetFileDifferences(oldGpioValues, newGpioValues);

            oldGpioValues = newGpioValues;

            foreach (var filePath in differences)
            {
                var pin = GetPin(filePath);

                UpdatePins(pin, filePath);
            }
        }

        private static void UpdatePins(GpioPin pin, string filePath)
        {
            var file = new FileInfo(filePath);

            if (file.Name == "value" || file.Name == "direction")
            {
                var direction = GetFileContents(file.Directory.FullName, "direction");
                SetDriveMode(pin, direction);

                var value = GetFileContents(file.Directory.FullName, "value");
                SetLogicLevel(pin, value);

                Debug.WriteLine($"Update Pin {pin.PinNumber} to {value}");
            }
            else
            {
                throw new NotSupportedException("This file is presently unknown and not processed by Bifrost");
            }
        }

        private static string GetFileContents(string filePath, string fileName)
        {
            var fileInfo = new FileInfo(Path.Combine(filePath, fileName));

            return File.ReadAllText(fileInfo.FullName);
        }

        private static void SetLogicLevel(GpioPin pin, string fileContent)
        {
            var logicLevel = (GpioPinValue)Enum.ToObject(typeof(GpioPinValue), Convert.ToInt16(fileContent));

            pin.Write(logicLevel);
        }

        private static void SetDriveMode(GpioPin pin, string fileContent)
        {
            if (fileContent == "out")
            {
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
            else
            {
                pin.SetDriveMode(GpioPinDriveMode.Input);
            }
        }

        private GpioPin GetPin(string path)
        {
            short pinNumber = GetPinNumber(path);

            if (pins.ContainsKey(pinNumber))
            {
                pin = pins[pinNumber];
            }
            else
            {
                pin = gpioController.OpenPin(pinNumber);
                pins.Add(pinNumber, pin);
            }

            return pin;
        }

        private static short GetPinNumber(string path)
        {
            var gpioText = "gpio";

            var firstIndexOfGpio = path.IndexOf(gpioText);

            var gpioDirectoryName = path.Substring(firstIndexOfGpio, path.Length - firstIndexOfGpio);

            var pinNumberString = gpioDirectoryName.Substring(0, gpioDirectoryName.IndexOf(@"\")).Replace(gpioText, "");

            return Convert.ToInt16(pinNumberString);
        }
    }
}
