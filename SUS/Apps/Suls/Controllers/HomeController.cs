using SUS.HTTP;
using SUS.MvcFramework;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [HttpGet("/")] //ne isma /Home/Index, a /
        public HttpResponse Index()
        {
            return this.View(); //Views/Home/Index.cshtml vikam
        }
    }
}
