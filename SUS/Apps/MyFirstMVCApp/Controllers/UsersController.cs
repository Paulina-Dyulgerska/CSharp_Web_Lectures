using SUS.HTTP;
using SUS.MvcFramework;
using System;

namespace MyFirstMVCApp.Controllers
{
    public class UsersController : Controller
    {
        //GET /Users/Login ////ne e nujno da utochnqwam pytq! zashtoto toj se vzima avtomatichno
        public HttpResponse Login()
        {
            //this is done by the base class Controller:
            //var responseHtml = File.ReadAllText("Views/Users/Login.html");
            //var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            //return new HttpResponse("text/html", responseBodyBytes);
            //return this.View("Views/Users/Login");

            return this.View();
        }

        [HttpPost("/Users/Login")] //posochvam pytq tuk, zashtoto Login e razlichno ot DoLogin!!!
        public HttpResponse DoLogin()
        {
            //TODO: read data
            //TODO: check user
            //TODO: log user

            //redirect home page
            return this.Redirect("/");
        }

        //GET /Users/Register //ne e nujno da utochnqwam pytq! zashtoto toj se vzima avtomatichno
        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost("/Users/Register")] //posochvam pytq tuk, zashtoto Register e razlichno ot DoRegister!!!
        public HttpResponse DoRegister()
        {
            return this.Redirect("/");
        }

            //GET /Users/Logout
            public HttpResponse Logout() //ne e nujno da utochnqwam pytq! zashtoto toj se vzima avtomatichno
            {
                if (!this.IsUserSignedIn())
                {
                    return this.Error("Only logged-in users can logout.");
                }

                this.SignOut();
                return this.Redirect("/");
            }
    }
}
