﻿using Suls.Services;
using Suls.ViewModel.Submissions;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly IProblemsService problemsService;
        private readonly ISubmissionsService submissionsService;

        public SubmissionsController(IProblemsService problemsService, ISubmissionsService submissionsService)
        {
            this.problemsService = problemsService;
            this.submissionsService = submissionsService;
        }

        public HttpResponse Create(string id)
        {
            var viewModel = new CreateViewModel
            {
                Name = this.problemsService.GetNameById(id),
                ProblemId = id,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(string problemId, string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length < 30 || code.Length > 800)
            {
                return this.Error("Code shoul be between 30 and 800 characters long.");
            }

            var userId = this.GetUserId();
            this.submissionsService.Create(problemId, userId, code);
            return this.Redirect("/");
        }

        public HttpResponse Delete(string id)
        {
            this.submissionsService.Delete(id);

            return this.Redirect("/Problems/Details");
        }
    }
}
