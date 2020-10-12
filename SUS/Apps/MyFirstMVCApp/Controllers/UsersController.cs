using SUS.HTTP;
using SUS.MvcFramework;
using System;

namespace MyFirstMVCApp.Controllers
{
    public class UsersController : Controller
    {
        public HttpResponse Login()
        {
            //this is done by the base class Controller:
            //var responseHtml = File.ReadAllText("Views/Users/Login.html");
            //var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            //return new HttpResponse("text/html", responseBodyBytes);
            //return this.View("Views/Users/Login");

            return this.View();
        }

        [HttpPost]
        public HttpResponse DoLogin()
        {
            //TODO: read data
            //TODO: check user
            //TODO: log user

            //redirect home page
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        public HttpResponse Logout()
        {
            return this.Redirect("/");
        }
    }
}
