using MyFirstMVCApp.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Runtime.CompilerServices;

namespace MyFirstMVCApp.Controllers
{
    public class HomeController : Controller
    {
        ////tozi Index beshe za igrachka, dolniq e spored zadanieto:
        ////[HttpGetAttribute("/")] //moga da go napisha taka:
        //[HttpGet("/")] //ako neshto e attribute, moga da ne pisha cqloto mu ime, a samo do predi Attribute!!!!!
        //public HttpResponse Index() //action
        //{
        //    //var responseHtml = "<h1>Welcome!<h1>" + request.Headers.FirstOrDefault(x => x.Name == "User-Agent")?.Value;
        //    //var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
        //    //var response = new HttpResponse("text/html", responseBodyBytes);
        //    //return response;

        //    //imam dostyp do requesta tuk:
        //    //Console.WriteLine(this.Request.Path);

        //    var viewModel = new IndexViewModel();
        //    viewModel.CurrentYear = DateTime.UtcNow.Year;
        //    viewModel.Message = "Welcome to Battle Cards";
        //    //ako sym bila na About page, to shte se syzdade Session s ime "about" i stojnost "yes" i veche kato izbera Home, shte mi 
        //    //se zarejda towa dopylnitelno syshtenie, che sym bila na about page i shte moga da prenasqm dori danni za usera po tozi
        //    //nachin!!!
        //    //if (this.Request.Session.ContainsKey("about"))
        //    if (this.IsUserSignedIn())
        //    {
        //        viewModel.Message += " WELCOME USER! YOU WERE ON THE ABOVE PAGE!";
        //    }
        //    return this.View(viewModel);
        //}

        //[HttpGetAttribute("/")] //moga da go napisha taka:
        [HttpGet("/")] //ako neshto e attribute, moga da ne pisha cqloto mu ime, a samo do predi Attribute!!!!!
        public HttpResponse Index() //action
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Cards/All");
            }
            return this.View();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] //tozi attribute e za da se pravi aggresive inlining pri compiliraneto na coda
        //i v IL coda da se zamestvat izrazite i methodite na tochnoto mqsto, na koeto se vikat v koda, a ne da stava prehvyrlqne po
        //coda i v IL coda, taka kakto e v moq C# cod!!! T.e. obratnoto na chetimostta kym koqto se stremq. Ama tq chetimostta e za horata 
        //i na praktika zabawq computrite!! Chrez tozi attribute si pravq optimizaciq na performenca!!!
        public HttpResponse About()
        {
            //this.Request.Session["about"] = "yes";
            this.SignIn("polq");//tova ako ne go mahna si ostavqm ogromen hack!!!
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
