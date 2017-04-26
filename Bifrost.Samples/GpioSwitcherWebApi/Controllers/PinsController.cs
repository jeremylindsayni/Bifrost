using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Bifrost.Devices.Gpio.Core;
using Bifrost.Devices.Gpio.Abstractions;
using Bifrost.Devices.Gpio;

namespace GpioSwitcherWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PinsController : Controller
    {
        private IGpioController gpioController;

        public PinsController()
        {
            Console.WriteLine("In controller - instantiating GpioController instance");
            gpioController = GpioController.Instance;
        }

        // GET api/pins
        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("About to list pin statuses.");
            return Ok(gpioController.Pins);
        }

        // GET api/pins/5
        [HttpGet("{pinId}")]
        public IActionResult Get(int pinId)
        {
            Console.WriteLine("About to get pin status.");
            var pin = gpioController.OpenPin(pinId);

            return Ok(pin.Read().ToString());
        }

        // POST api/pins
        [HttpPost]
        public void SwitchPin(int pinId, int status)
        {
            Console.WriteLine("About to change pin status.");
            var pin = gpioController.OpenPin(pinId);

            pin.SetDriveMode(GpioPinDriveMode.Output);

            if (status == 1) {
                Console.WriteLine("Going on");
                pin.Write(GpioPinValue.High);
            } else {
                Console.WriteLine("Going off");
                pin.Write(GpioPinValue.Low);
            }
        }
    }
}
