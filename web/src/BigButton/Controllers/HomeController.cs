using System;
using Microsoft.AspNetCore.Mvc;
using BigButton.Azure;
using BigButton.Models;
using Microsoft.Extensions.Options;
using NUglify.Css;

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
                var hub = new IotHub(_iotConfig.Value);
                hub.SendMessage(color);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
