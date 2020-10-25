using Suls.Data;
using Suls.ViewModel.Problems;
using System.Collections.Generic;
using System.Linq;

namespace Suls.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly ApplicationDbContext db;

        public ProblemsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Create(string name, ushort points)
        {
            this.db.Problems.Add(new Problem { Name = name, Points = points });
            this.db.SaveChanges();
        }

        public IEnumerable<HomePageProblemViewModel> GetAll()
        {
            return this.db.Problems.Select(x => new HomePageProblemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Count = x.Submissions.Count(),
            }).ToList();
        }

        public string GetNameById(string id)
        {
            //return this.db.Problems.FirstOrDefault(x => x.Id == id).Name; //po-dobyr variant e tozi, zashtoto nqma da drupna
            //celiqt zapis, a samo Name!!
            return this.db.Problems.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
        }

        public ProblemViewModel GetAllByProblemId(string id)
        {
            //var submissions = this.db.Submissions.Where(x => x.ProblemId == id).Select(x => new SubmissionViewModel
            //{
            //    SubmissionId = x.Id,
            //    Username = x.User.Username,
            //    AchievedResult = x.AchievedResult,
            //    CreatedOn = x.CreatedOn,
            //    MaxPoints = x.Problem.Points,
            //});

            //var problemName = this.GetNameById(id);

            //return new ProblemViewModel
            //{
            //    Name = problemName,
            //    Submissions = submissions,
            //};

            //moje i taka:
            return this.db.Problems.Where(x => x.Id == id).Select(x => new ProblemViewModel
            {
                Name = x.Name,
                Submissions = x.Submissions.Select(s => new SubmissionViewModel
                {
                    SubmissionId = s.Id,
                    Username = s.User.Username,
                    AchievedResult = s.AchievedResult,
                    CreatedOn = s.CreatedOn,
                    MaxPoints = s.Problem.Points,
                }).ToList(),
            }).FirstOrDefault();
        }
    }
}
