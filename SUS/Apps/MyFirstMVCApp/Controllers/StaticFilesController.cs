using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMVCApp.Controllers
{
    public class StaticFilesController : Controller
    {
        public HttpResponse Favicon(HttpRequest request)
        {
            //var fileBytes = System.IO.File.ReadAllBytes("wwwroot/favicon.ico");
            ////var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            //var response = new HttpResponse("image/x-icon", fileBytes);
            //return response;

            return this.File("wwwroot/favicon.ico", "image/x-icon");
        }
        public HttpResponse BootstrapCss(HttpRequest request)
        {
            return this.File("wwwroot/css/bootstrap.min.css", "text/css");
        }
        public HttpResponse CustomCss(HttpRequest request)
        {
            return this.File("wwwroot/css/custom.css", "text/css");
        }
        public HttpResponse BootstrapJs(HttpRequest request)
        {
            return this.File("wwwroot/js/bootstrap.bundle.min.js", "text/javascript");
        }
        public HttpResponse CustomJs(HttpRequest request)
        {
            return this.File("wwwroot/js/custom.js", "text/javascript");
        }
    }
}
