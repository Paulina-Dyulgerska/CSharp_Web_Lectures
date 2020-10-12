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

        public HttpResponse View(object viewModel = null, [CallerMemberName] string viewPath = null)
        {
            //imam dostyp do requesta tuk:
            Console.WriteLine($"This is the Http Request Path => {this.Request.Path}");


            var viewContent = System.IO.File.ReadAllText($"Views/" +
                $"{this.GetType().Name.Replace("Controller", string.Empty)}/" +
                $"{viewPath}.cshtml");
            viewContent = this.viewEngine.GetHtml(viewContent, viewModel);

            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "___VIEW_GOES_HERE___");

            var responseHtml = layout.Replace("___VIEW_GOES_HERE___", viewContent);
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }

        public HttpResponse File(string filePath, string contentType)
        {
            //var fileBytes = System.IO.File.ReadAllBytes("wwwroot/favicon.ico");
            //var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            //var response = new HttpResponse("image/x-icon", fileBytes);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new HttpResponse(contentType, fileBytes);

            return response;
        }

        public HttpResponse Redirect(string url)
        {
            var response = new HttpResponse(HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", url));
            return response;
        }
    }
}