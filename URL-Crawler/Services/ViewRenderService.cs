using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace URL_Crawler.Services
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(IRazorViewEngine razorViewEngine,
                                 ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = razorViewEngine ?? throw new ArgumentNullException(nameof(razorViewEngine));
            _tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
        }

        /// <summary>
        /// Render view invoking from Controller, passing existing context.
        /// </summary>
        /// <param name="viewName">The name of the view to render.</param>
        /// <param name="model">The model the view requires.</param>
        /// <param name="context">The ControllerContext of the invoking controller.</param>
        /// <param name="isPartialView">If the view being found is a partial view.</param>
        /// <returns>The rendered HTML after the view has been processed.</returns>
        public async Task<string> RenderView(string viewName, object model, ControllerContext context, bool isPartialView = false)
        {
            await using var stringWriter = new StringWriter();
            var viewResult = FindView(context, viewName, isPartialView);

            if (viewResult == null)
                throw new ArgumentNullException($"{viewName} was unable to be found.");

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                Model = model
            };

            var viewContext = new ViewContext(
                context,
                viewResult,
                viewDictionary,
                new TempDataDictionary(context.HttpContext, _tempDataProvider),
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.RenderAsync(viewContext);

            return stringWriter.ToString();
        }

        private IView FindView(ActionContext actionContext, string viewName, bool isPartialView = false)
        {
            var getViewResult = _razorViewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: !isPartialView);
            if (getViewResult.Success)
                return getViewResult.View;

            // Currently broken, see https://github.com/dotnet/aspnetcore/issues/38373. Adding regardless for when it is fixed.
            var findViewResult = _razorViewEngine.FindView(actionContext, viewName, isMainPage: !isPartialView);
            if (findViewResult.Success)
                return findViewResult.View;

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }
    }
}