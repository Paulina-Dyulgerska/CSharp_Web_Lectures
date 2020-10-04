using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMVCApp.Controllers
{
   public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request) //action
        {
            //var responseHtml = "<h1>Welcome!<h1>" + request.Headers.FirstOrDefault(x => x.Name == "User-Agent")?.Value;
            //var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            //var response = new HttpResponse("text/html", responseBodyBytes);
            //return response;

            return this.View();
        }

        //No About page in my new web application!
        //public HttpResponse About(HttpRequest request)
        //{
        //    var responseHtml = "<h1>About...<h1>";
        //    var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
        //    var response = new HttpResponse("text/html", responseBodyBytes);
        //    return response;
        //}
    }
}
