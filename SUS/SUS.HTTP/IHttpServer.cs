using System.Threading.Tasks;

namespace SUS.HTTP
{
   public interface IHttpServer
    {
        //void AddRoute(string path, Func<HttpRequest, HttpResponse> action); //action is the Func that is executed when the path is choosed

        Task StartAsync(int port = 80);
    }
}
