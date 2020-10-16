using SUS.HTTP;
using SUS.MvcFramework.ViewEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SUS.MvcFramework
{
    public abstract class Controller
    {
        private const string UserIdSessionName = "UserId";
        private SusViewEngine viewEngine;

        public Controller()
        {
            this.viewEngine = new SusViewEngine();
        }

        public HttpRequest Request { get; set; }
        //HttpRequesta veche e chast ot Controllerite, a ne se podawa otvyn na vseki edin method izrishno!!!!


        //public HttpResponse View()
        //{
        //    var responseHtml = System.IO.File.ReadAllText($"Views/" +
        //        $"{this.GetType().Name.Replace("Controller", string.Empty)}/" +
        //        $"{(new StackTrace()).GetFrame(1).GetMethod().Name}.html");
        //    var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
        //    var response = new HttpResponse("text/html", responseBodyBytes);

        //    return response;
        //}

        protected HttpResponse View(object viewModel = null, [CallerMemberName] string viewPath = null)
        {
            //imam dostyp do requesta tuk:
            Console.WriteLine($"This is the Http Request Path => {this.Request.Path}");


            var viewContent = System.IO.File.ReadAllText($"Views/" +
                $"{this.GetType().Name.Replace("Controller", string.Empty)}/" +
                $"{viewPath}.cshtml");
            viewContent = this.viewEngine.GetHtml(viewContent, viewModel);

            var responseHtml = PutViewInLayout(viewContent, viewContent);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }

        protected HttpResponse File(string filePath, string contentType)
        {
            //var fileBytes = System.IO.File.ReadAllBytes("wwwroot/favicon.ico");
            //var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            //var response = new HttpResponse("image/x-icon", fileBytes);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new HttpResponse(contentType, fileBytes);

            return response;
        }

        protected HttpResponse Redirect(string url)
        {
            var response = new HttpResponse(HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", url));
            return response;
        }

        protected HttpResponse Error(string errorText)
        {
            var viewContent = $"<div class=\"alert alert-danger text-center\" role=\"alert\">{errorText}</div>";
            var layout = this.PutViewInLayout(viewContent);
            var responseBodyBytes = Encoding.UTF8.GetBytes(layout);
            var response = new HttpResponse("text/html", responseBodyBytes, HttpStatusCode.ServerError);

            return response;
        }

        //dolnite 4 methoda sa dostypa na vseki edin Controller do towa da signIn-va user, da go signOut-va, da mu vizma Id-to i da mi kazva
        //dali e lognat user.
        protected void SignIn(string userId)
        {
            this.Request.Session[UserIdSessionName] = userId; //zapisvam v sessiona id-to na usera
        }

        protected void SignOut()
        {
            this.Request.Session[UserIdSessionName] = null; //iztrivam ot sessiona id-to na usera
            //pri prisvoqvane na stojnost na daden KEY ot Dictionaryto, ne e nujno da pravq proverka dali ima takyv KEY ili nqma, zashtoto
            //ako go ima, shte se overwrite-ne,a ako go nqma - shte se napravi takawa Key-Value dvojka.
         }

        protected bool IsUserSignedIn() => this.Request.Session.ContainsKey(UserIdSessionName);
        //Dictionaryto gyrmi, ako iskam da vzema stojnost ot nesyshtestvuvasht KEY!!!!
        //ako poskam direktno towa da se proveri: this.Request.Session[UserIdSessionName !=null, mi gyrmi zashtoto 
        //NQMA TAKYV KEY pri pyrvonachalnoto zarejdane na Home page!!!!! Zatowa da proverqwam dali ima, a ne da iskam da mu vzema stojnostta!!!!!

        protected string GetUserId() => this.Request.Session.ContainsKey(UserIdSessionName) ?
            this.Request.Session[UserIdSessionName] : null; //za da ne gyrmi Dictionaryto, trqbwa PYRVO da proverq ima li takyv zapis s takyv KEY
        //I sled towa da go vzimam, inache da mi vyrne null!!!!

        private string PutViewInLayout(string viewContent, object viewModel = null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "___VIEW_GOES_HERE___");
            layout = this.viewEngine.GetHtml(layout, viewModel);
            var responseHtml = layout.Replace("___VIEW_GOES_HERE___", viewContent);

            return responseHtml;
        }
    }
}