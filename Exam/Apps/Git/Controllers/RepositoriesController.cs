using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoriesService repositoriesService;

        public RepositoriesController(IRepositoriesService repositoriesService)
        {
            this.repositoriesService = repositoriesService;
        }

        public HttpResponse All()
        {
            var viewModel = this.repositoriesService.GetAll();
            return this.View(viewModel);
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string repositoryType)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 10)
            {
                return this.Error("Name is required and should be between 3 and 10 characters long.");
            }

            if (string.IsNullOrWhiteSpace(repositoryType))
            {
                return this.Error("Repository type is required. Please choose type form the menu: public or private.");
            }

            var userId = this.GetUserId();

            this.repositoriesService.Create(name, repositoryType, userId);

            return this.Redirect("/Repositories/All");
        }
    }
}
