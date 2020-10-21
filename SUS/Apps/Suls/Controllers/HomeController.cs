using Suls.ViewModel.Problems;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Collections.Generic;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")] //ne isma /Home/Index, a /
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                return this.View(new List<HomePageProblemViewModel>(), "IndexLoggedIn");
            }
            else
            {
                return this.View(); //Views/Home/Index.cshtml vikam
            }
        }
    }
}
