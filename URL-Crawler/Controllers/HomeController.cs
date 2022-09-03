using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using URL_Crawler.Models;
using URL_Crawler.Services;

namespace URL_Crawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IViewRenderService viewRenderService, ILogger<HomeController> logger)
        {
            _viewRenderService = viewRenderService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new UrlFormViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetUrlContentAsync(UrlFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Render partial server side to get ModelState validation on partial, allowing reuse.
                    var renderedPartial = await _viewRenderService.RenderView("Views/Home/_UrlForm.cshtml", model, ControllerContext, true);
                    return Json(renderedPartial);
                }

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}