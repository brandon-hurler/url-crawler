using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using URL_Crawler.Models.Adapters;
using URL_Crawler.Models.Interfaces;
using URL_Crawler.Models.ViewModels;
using URL_Crawler.Services;

namespace URL_Crawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IViewRenderService _viewRenderService;
        private readonly IDocumentReader<HtmlWebDocumentReader> _documentReader;

        public HomeController(IDocumentReader<HtmlWebDocumentReader> documentReader, IViewRenderService viewRenderService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _documentReader = documentReader;
            _viewRenderService = viewRenderService;
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
                    var urlFormPartial = await _viewRenderService.RenderView("Views/Home/_UrlForm.cshtml", model, ControllerContext, true);
                    return Json(new { success = false, payload = urlFormPartial });
                }

                // Can swap for a different reader type, e.g. file reader by changing injection above.
                var document = _documentReader.Read(model.Url);

                var contentModel = new ContentViewModel(document, model.TopWordCount ?? 10);
                var contentPartial = await _viewRenderService.RenderView("Views/Home/_Content.cshtml", contentModel, ControllerContext, true);

                return Json(new { success = true, payload = contentPartial });
            }
            catch (WebException)
            {
                var contentPartial = await _viewRenderService.RenderView("Views/Home/_UrlError.cshtml", string.Empty, ControllerContext, true);

                // Success true for caught error for invalid URL.
                return Json(new { success = true, payload = contentPartial });
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