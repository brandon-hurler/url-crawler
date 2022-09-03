using Microsoft.AspNetCore.Mvc;

namespace URL_Crawler.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderView(string viewName, object model, ControllerContext context, bool isPartialView);
    }
}