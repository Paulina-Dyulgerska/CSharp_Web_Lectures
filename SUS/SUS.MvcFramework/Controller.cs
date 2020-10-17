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
            viewContent = this.viewEngine.GetHtml(viewContent, viewModel, this.GetUserId());

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

        protected bool IsUserSignedIn() => this.Request.Session.ContainsKey(UserIdSessionName)
            && this.Request.Session[UserIdSessionName] != null;
        //Dictionaryto gyrmi, ako iskam da vzema stojnost ot nesyshtestvuvasht KEY!!!!
        //ako poskam direktno towa da se proveri: this.Request.Session[UserIdSessionName] !=null, mi gyrmi zashtoto 
        //NQMA TAKYV KEY pri pyrvonachalnoto zarejdane na Home page!!!!! Zatowa da proverqwam dali ima, a ne da iskam da mu vzema stojnostta!!!!!
        //mnogo vajna tuk e i proverkata dali, ako ima takyv KEY toj e null, zashoto ako usera se logne, posle se razlogne, toj 
        //ima takova Session cookie, no to e null, zashoto az go nuliram pri logout! No ako proverqwam samo dali ima takowa cookie,
        //to shte izleze, che ima dori i ako vlaue-to zad nego e null!!! Zatowa proverqwam i dvete usloviq kato e mnogo vajno da
        //proverqwam pyrvo za syshtestvuwaneto na UserIdSessionName i edva sled towa za towa dali e null, zashtoto ako ne syshtestvuva
        //to shte mi prekysne tam proverkata i nqma da mi gyrmi dictionaryto, kakto ako sloja proverkata na obratno:
        //this.Request.Session[UserIdSessionName] != null && this.Request.Session.ContainsKey(UserIdSessionName)!!!!

        protected string GetUserId() => this.Request.Session.ContainsKey(UserIdSessionName) ?
            this.Request.Session[UserIdSessionName] : null; //za da ne gyrmi Dictionaryto, trqbwa PYRVO da proverq ima li takyv zapis s takyv KEY
        //I sled towa da go vzimam, inache da mi vyrne null!!!!

        private string PutViewInLayout(string viewContent, object viewModel = null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "___VIEW_GOES_HERE___");
            layout = this.viewEngine.GetHtml(layout, viewModel, this.GetUserId());
            var responseHtml = layout.Replace("___VIEW_GOES_HERE___", viewContent);

            return responseHtml;
        }
    }
}