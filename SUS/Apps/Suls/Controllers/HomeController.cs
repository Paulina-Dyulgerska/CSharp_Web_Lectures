using Suls.Services;
using SUS.HTTP;
using SUS.MvcFramework;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProblemsService problemsService;

        public HomeController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        [HttpGet("/")] //ne isma /Home/Index, a /
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                var viewModel = this.problemsService.GetAll();
                return this.View(viewModel, "IndexLoggedIn");
            }
            else
            {
                return this.View(); //Views/Home/Index.cshtml vikam
            }
        }
    }
}
