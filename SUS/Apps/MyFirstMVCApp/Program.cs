using MyFirstMVCApp.Controllers;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstMVCApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Route> routeTable = new List<Route>();

            //routes for the dynamic views:
            routeTable.Add(new Route("/", new HomeController().Index));
            routeTable.Add(new Route("/users/login", new UsersController().Login));
            routeTable.Add(new Route("/users/register", new UsersController().Register));
            routeTable.Add(new Route("/cards/add", new CardsController().Add));
            routeTable.Add(new Route("/cards/all", new CardsController().All));
            routeTable.Add(new Route("/cards/collection", new CardsController().Collection));

            //routes for the static files:
            routeTable.Add(new Route("/favicon.ico", new StaticFilesController().Favicon));
            routeTable.Add(new Route("/css/bootstrap.min.css", new StaticFilesController().BootstrapCss));
            routeTable.Add(new Route("/css/custom.css", new StaticFilesController().CustomCss));
            routeTable.Add(new Route("/js/bootstrap.bundle.min.js", new StaticFilesController().BootstrapJs));
            routeTable.Add(new Route("/js/custom.js", new StaticFilesController().CustomJs));

            //route table before List<Route>:
            //server.AddRoute("/", new HomeController().Index);
            //server.AddRoute("/favicon.ico", new StaticFilesController().Favicon);
            //server.AddRoute("/users/login", new UsersController().Login);
            //server.AddRoute("/users/register",new UsersController().Register);
            //server.AddRoute("/cards/add",new CardsController().Add);
            //server.AddRoute("/cards/all",new CardsController().All);
            //server.AddRoute("/cards/collection",new CardsController().Collection);

            await Host.CreateHostAsync(routeTable, 12345);
        }
    }
}
