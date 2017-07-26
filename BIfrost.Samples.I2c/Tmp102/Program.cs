using System;

namespace Tmp102
{
    class Program
    {
        static void Main(string[] args)
        {
            var temperatureSensor = new Tmp102(A0PinConnection.Ground);

            temperatureSensor.Initialize();

            var temperatureInCelcius = temperatureSensor.GetTemperature(Scale.Celcius);

            var temperatureInFahrenheit = temperatureSensor.GetTemperature(Scale.Fahrenheit);

            Console.WriteLine($"Temperature (C): {temperatureInCelcius}");

            Console.WriteLine($"Temperature (F): {temperatureInFahrenheit}");
        }
    }
}