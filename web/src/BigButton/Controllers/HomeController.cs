using System;
using Microsoft.AspNetCore.Mvc;
using BigButton.Azure;
using BigButton.Models;
using Microsoft.Extensions.Options;

namespace BigButton.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<IotHubConfiguration> _iotConfig;

        public HomeController(IOptions<IotHubConfiguration> iotConfig)
        {
            _iotConfig = iotConfig;
        }

        public IActionResult Index()
        {
            return View(ColorModel.Create());
        }

        [HttpPost]
        [Route("/Home/Button/{color}")]
        public IActionResult Button(string color)
        {
            try
            {
                if(FalseColor(color)) throw new Exception($"Vals speler!!! '{color}' wordt alleen gebruikt door de echte knop!");
                var hub = new IotHub(_iotConfig.Value);
                hub.SendMessage(color);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        bool FalseColor(string color)
        {
            if (color.Equals("ESP Red", StringComparison.OrdinalIgnoreCase)) return true;
            if (color.Equals("ESP Blue", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
