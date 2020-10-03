using SUS.HTTP;
using SUS.MvcFramework;
using System.IO;

namespace MyFirstMVCApp.Controllers
{
   public class StaticFilesController : Controller
    {
        public HttpResponse Favicon(HttpRequest request)
        {
            var fileBytes = File.ReadAllBytes("wwwroot/favicon.ico");
            //var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            var response = new HttpResponse("image/x-icon", fileBytes);

            return response;
        }
    }
}
