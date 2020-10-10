using SUS.HTTP;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port)
        {
            List<Route> routeTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routeTable);

            IHttpServer server = new HttpServer(routeTable);

            //foreach (var route in routeTable)
            //{
            //    server.AddRoute(route.Path, route.Action);
            //}

            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", $"http://localhost:{port}");

            await server.StartAsync(port);
        }
    }
}
