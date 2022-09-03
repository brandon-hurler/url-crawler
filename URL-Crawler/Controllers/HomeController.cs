using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using URL_Crawler.Models;

namespace URL_Crawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new UrlFormViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult GetUrlContent(UrlFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("_UrlForm", model);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}