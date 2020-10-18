using SUS.HTTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port)
        {
            List<Route> routeTable = new List<Route>();
            IServiceCollection serviceCollection = new ServiceCollection();

            application.ConfigureServices(serviceCollection);
            application.Configure(routeTable);

            AutoRegisterStaticFile(routeTable);
            AutoRegisterRoutes(routeTable, application, serviceCollection);

            PrintRouteTable(routeTable);

            IHttpServer server = new HttpServer(routeTable);

            //foreach (var route in routeTable)
            //{
            //    server.AddRoute(route.Path, route.Action);
            //}

            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", $"http://localhost:{port}");

            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application, IServiceCollection serviceCollection)
        {
            //routeTable.Add(new Route("/users/register", HttpMethod.Get, new UsersController().Register));
            var controllerTypes = application
                 .GetType().Assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));

            foreach (var controllerType in controllerTypes)
            {
                Console.WriteLine(controllerType.Name);

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
                                     ExecuteAction(serviceCollection, controllerType, method, request)));

                    Console.WriteLine($" - {method.Name}");
                }
            }
        }

        private static HttpResponse ExecuteAction
            (IServiceCollection serviceCollection, Type controllerType, MethodInfo action, HttpRequest request)
        {
            //var instance = Activator.CreateInstance(controllerType) as Controller;
            //vmesto s Activator, shte si pravq veche controllerite sys serviceCollection:
            var instance = serviceCollection.CreateInstance(controllerType) as Controller;
            instance.Request = request;
            //tuk shte vzema argumentite, koito da podam na actiona!!!!
            var arguments = new List<object>();
            var parameters = action.GetParameters();
            foreach (var parameter in parameters)
            {
                var httpParameterValue = GetParameterFromRequest(request, parameter.Name);
                //iskam da mi conventira typa na polucheniq ot forma ili querystring parameter, kym typa na parametyra sys syshtoto
                //ime, kojto parameter se iska ot actiona (methoda izvikvan ot requesta)!!!!
                //parameter.ParameterType e typa na parametera ot methoda, a httpParameterValue e stringovata stojnost na 
                //parametera, kojto mi e doshil s forma ili querystring ot requesta ot usera!!!
                var parameterValue = Convert.ChangeType(httpParameterValue, parameter.ParameterType);
                //Convert NE RAZBIRA ot slojni types, a samo ot primitivni types kato int, string i t.n.
                //ako iskam da moga da rabotq s AddCardInputModel, vmesto sys 100 parameters pootdelno, trqbwa da mu kaja kym
                //kakwo i kak da convertne:
                //Convert vrysth null, ako ne uspee da convertne primitiven type, samo za string e vyzmojno da vyrne null kato 
                //stojnost na stringa, t.e. ako imam null i ne e string type, znachi parameter.parameterType e ot slojen type!!!!
                if (parameterValue == null && parameter.ParameterType != typeof(string))
                {
                    //complex type:
                    //pravq instanciq na complex type-to; vzimam vsichki propertyta na tozi complex type object;
                    //za vsqko property ot complex type-to wzimam onzi parameter ot requesta, kojto ima syshtoto ime kato propertyto;
                    //Convertvam valueto na propertyto ot requesta kym type-to na propertyto ot complex objecta;
                    //zapisvam valueto na namereniq parameter ot requesta v syotvetnoto property sys syshtoto ime;
                    parameterValue = Activator.CreateInstance(parameter.ParameterType);
                    var properties = parameter.ParameterType.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyHttpParameterValue = GetParameterFromRequest(request, property.Name);
                        var propertyParameterValue = Convert.ChangeType(propertyHttpParameterValue, property.PropertyType);
                        property.SetValue(parameterValue, propertyParameterValue);
                    }
                }
                arguments.Add(parameterValue);
            }

            //HttpRequesta e v request i toj veche e chast ot Controllerite, a ne se podawa otvyn na vseki edin method izrishno!!!!
            var response = action.Invoke(instance, arguments.ToArray()) as HttpResponse;
            //pri nas vseki method vryshta HttpResponse.
            //tuk moga da podam array s argumenti, koito da se podadat na actiona!!!!
            //tezi arguments sym gi vzela ot Request.FormData!!!! Ako vyv requesta imam podadeni danni, to tezi danni shte otidat
            //kato parameters na vikaniq ot requesta action (methoda, kojto obslujva requesta)!!!!!!!!
            //v ASP>NET Core vseki method vryshta IActionResult, tova ni e malkata razlika s ASP.NET Core.
            return response;
        }

        private static string GetParameterFromRequest(HttpRequest request, string parameterName)
        {
            if (request.FormData.Any(x=> x.Key.ToLower() == parameterName.ToLower()))
            {
                return request.FormData[parameterName.ToLower()];
            }

            if (request.QueryData.Any(x => x.Key.ToLower() == parameterName.ToLower()))
            {
                return request.QueryData[parameterName.ToLower()];
            }

            //ASP.NET tyrsi ne samo v FormData i QueryData, no i v Headers, ama nie nqma da tyrsim tam.

            return null;
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
