using Suls.Services;
using SUS.HTTP;
using SUS.MvcFramework;

namespace Suls.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, int points)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 5 || name.Length >20)
            {
                return this.Error("Name should be between 5 and 20 characters long.");
            }

            if (points < 50 || points > 300)
            {
                return this.Error("Points should be an integer number between 50 and 300.");
            }


            this.problemsService.Create(name, (ushort)points);

            return this.Redirect("/");
        }

        public HttpResponse Details(string id)
        {
            var viewModel = this.problemsService.GetAllByProblemId(id);

            return this.View(viewModel);
        }
    }
}
