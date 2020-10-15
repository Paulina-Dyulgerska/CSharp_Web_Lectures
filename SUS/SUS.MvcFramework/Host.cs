using SUS.HTTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port)
        {
            List<Route> routeTable = new List<Route>();

            AutoRegisterStaticFile(routeTable);
            AutoRegisterRoutes(routeTable, application);

            application.ConfigureServices();
            application.Configure(routeTable);

            PrintRouteTable(routeTable);

            IHttpServer server = new HttpServer(routeTable);

            //foreach (var route in routeTable)
            //{
            //    server.AddRoute(route.Path, route.Action);
            //}

            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", $"http://localhost:{port}");

            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application)
        {
            //routeTable.Add(new Route("/users/register", HttpMethod.Get, new UsersController().Register));
            var controllerTypes = application
                 .GetType().Assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));

            foreach (var controllerType in controllerTypes)
            {
                System.Console.WriteLine(controllerType.Name);

                //iskam samo methodite, koito naricham actions v edno MVC, t.e. krajni destinacii za potrebitelski zaqwki!!
                var methods = controllerType.GetMethods()
                    .Where(x => x.IsPublic
                    && !x.IsStatic
                    && x.DeclaringType == controllerType //samo methods, kojto sa declarirani v syotvetniq controller class iskam, ne v base classa mu!
                    && !x.IsAbstract
                    && !x.IsConstructor //ne e constructor method
                    && !x.IsSpecialName); //ne e get ili set method na property!!! te sa s imena get_PropertyName i set_Propertyname, ne gi iskam!

                foreach (var method in methods)
                {
                    var url = "/" + controllerType.Name.Replace("Controller", string.Empty) + "/" + method.Name;

                    var attribute = method
                        .GetCustomAttributes(false)
                        .FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute))) as BaseHttpAttribute;

                    var httpMethod = HttpMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (!string.IsNullOrEmpty(attribute?.Url))
                    {
                        url = attribute.Url;
                    }

                    routeTable.Add(new Route(url, httpMethod, (request) =>
                    {
                        var instance = Activator.CreateInstance(controllerType) as Controller;
                        instance.Request = request;
                        //HttpRequesta e v request i toj veche e chast ot Controllerite, a ne se podawa otvyn na vseki edin method izrishno!!!!
                        var response = method.Invoke(instance, new object[] { }) as HttpResponse; //pri nas vseki method vryshta HttpResponse
                        //v ASP>NET Core vseki method vryshta IActionResult, tova ni e malkata razlika s ASP.NET Core.
                        return response;
                    }));

                    System.Console.WriteLine($" - {method.Name}");
                }
            }
        }

        private static void PrintRouteTable(List<Route> routeTable)
        {
            //za moe info pri start na Consolnoto Appche, shte se printvat vischki routes ot RouteTable na Console:
            System.Console.WriteLine("---All registered routes---");
            foreach (var route in routeTable)
            {
                System.Console.WriteLine($"{route.Method} {route.Path}");
            }
            System.Console.WriteLine("---End of routes in route table---");
        }

        private static void AutoRegisterStaticFile(List<Route> routeTable)
        {
            //vzemi mi recursivno vsichki files ot tazi dir: Pisha path kym directoriqta relativno, relativno sprqmo
            //papkata, v koqto se namira prilojenieto, koeto sym startirala. Prilojenieto e startirano s program.cs, a wwwroot e na 
            //edno nivo s faila program.cs, zatova directno q posochwam i shte se nameri tochno tazi, koqto iskam!!!
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);

            foreach (var staticFile in staticFiles)
            {
                var url = staticFile.Replace("wwwroot", string.Empty).Replace("\\", "/");

                routeTable.Add(new Route(url, HttpMethod.Get, (request) =>
                {
                    var fileContent = File.ReadAllBytes(staticFile);
                    var fileExtention = new FileInfo(staticFile).Extension;
                    var contentType = fileExtention switch
                    {
                        ".txt" => "text/plain",
                        ".js" => "text/javascript",
                        ".css" => "text/css",
                        ".jgp" => "image/jpg",
                        ".jgep" => "image/jpg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".ico" => "image/x-icon",
                        ".html" => "text/html",
                        _ => "text/plain", //tova e defaultnata stojnost, t.e. ako ne e nikoe ot gornite, togawa se vryshta nejniqt return.
                    };

                    return new HttpResponse("", fileContent, HttpStatusCode.Ok);
                }));
            }

            //StaticFileController classa stana izlishen, zashtoto vsichko towa go pravq v Host avtomatichno s gorniq code!!!!!!
        }
    }
}
