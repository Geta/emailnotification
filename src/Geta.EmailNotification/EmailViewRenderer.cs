using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Geta.EmailNotification
{
    /// <summary>
    /// Renders <see cref="Email"/> view's into raw strings using the MVC ViewEngine infrastructure.
    /// Source: https://github.com/andrewdavey/postal
    /// </summary>
    public class EmailViewRenderer : IEmailViewRenderer
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public EmailViewRenderer(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Creates a new <see cref="IEmailViewRenderer"/> that uses the given view engines.
        /// </summary>
        /// <param name="viewEngines">The view engines to use when rendering email views.</param>
        // public EmailViewRenderer(ViewEngineCollection viewEngines)
        // {
        //     this.viewEngines = viewEngines;
        //     EmailViewDirectoryName = "Emails";
        // }
        //
        // readonly ViewEngineCollection viewEngines;

        /// <summary>
        /// The name of the directory in "Views" that contains the email views.
        /// By default, this is "Emails".
        /// </summary>
        // public string EmailViewDirectoryName { get; set; }

        /// <summary>
        /// Renders an email view.
        /// </summary>
        /// <param name="email">The email to render.</param>
        /// <param name="viewName">Optional email view name override. If null then the email's ViewName property is used instead.</param>
        /// <returns>The rendered email view output.</returns>
        // public string Render(EmailNotificationRequest email, string viewName = null)
        // {
        //     viewName = viewName ?? email.ViewName;
        //     var controllerContext = CreateControllerContext();
        //     var view = CreateView(viewName, controllerContext);
        //     var viewOutput = RenderView(view, email.ViewData, controllerContext);
        //     return viewOutput;
        // }
        
        public async Task<string> RenderAsync(EmailNotificationRequest model, string viewName)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
     
            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
     
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }
     
                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };
     
                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );
     
                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        // ControllerContext CreateControllerContext()
        // {
        //     // A dummy HttpContextBase that is enough to allow the view to be rendered.
        //     var httpContext = new HttpContextWrapper(
        //         new HttpContext(
        //             new HttpRequest("", UrlRoot(), ""),
        //             new HttpResponse(TextWriter.Null)
        //         )
        //     );
        //     var routeData = new RouteData();
        //     routeData.Values["controller"] = EmailViewDirectoryName;
        //     var requestContext = new RequestContext(httpContext, routeData);
        //     var stubController = new StubController();
        //     var controllerContext = new ControllerContext(requestContext, stubController);
        //     stubController.ControllerContext = controllerContext;
        //     return controllerContext;
        // }

        // string UrlRoot()
        // {
        //     var httpContext = HttpContext.Current;
        //     if (httpContext == null)
        //     {
        //         return "http://localhost";
        //     }
        //
        //     return httpContext.Request.Url.GetLeftPart(UriPartial.Authority) +
        //            httpContext.Request.ApplicationPath;
        // }

        // IView CreateView(string viewName, ControllerContext controllerContext)
        // {
        //     var result = viewEngines.FindView(controllerContext, viewName, null);
        //     if (result.View != null)
        //         return result.View;
        //
        //     throw new Exception(
        //         "Email view not found for " + viewName +
        //         ". Locations searched:" + Environment.NewLine +
        //         string.Join(Environment.NewLine, result.SearchedLocations)
        //     );
        // }

        // string RenderView(IView view, ViewDataDictionary viewData, ControllerContext controllerContext)
        // {
        //     using (var writer = new StringWriter())
        //     {
        //         var viewContext = new ViewContext(controllerContext, view, viewData, new TempDataDictionary(), writer);
        //         view.Render(viewContext, writer);
        //         return writer.GetStringBuilder().ToString();
        //     }
        // }
        //
        // // StubController so we can create a ControllerContext.
        // class StubController : Controller { }
    }
}