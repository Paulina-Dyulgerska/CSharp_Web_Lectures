using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMVCApp.Controllers
{
  public  class UsersController : Controller
    {
        public HttpResponse Login(HttpRequest request)
        {
            //this is done by the base class Controller:
            //var responseHtml = File.ReadAllText("Views/Users/Login.html");
            //var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            //return new HttpResponse("text/html", responseBodyBytes);
            //return this.View("Views/Users/Login");

            return this.View();
        }
        public HttpResponse Register(HttpRequest request)
        {
            return this.View();
        }

    }
}
