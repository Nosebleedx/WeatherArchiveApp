using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherArchiveApp.Models;
using WeatherArchiveApp.Services;

namespace WeatherArchiveApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WeatherDataService _weatherDataService;
        private const int PageSize = 20;

        public HomeController(ILogger<HomeController> logger, WeatherDataService weatherDataService)
        {
            _logger = logger;
            _weatherDataService = weatherDataService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadArchives()
        {
            return View();
        }

        public IActionResult PrivacyAsync()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
