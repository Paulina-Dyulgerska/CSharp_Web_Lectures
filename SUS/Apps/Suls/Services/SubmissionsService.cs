using Suls.Data;
using System;
using System.Linq;

namespace Suls.Services
{
    public class SubmissionsService : ISubmissionsService
    {
        private readonly ApplicationDbContext db;
        private readonly Random random;

        public SubmissionsService(ApplicationDbContext db, Random random)
        {
            this.db = db;
            this.random = random;
        }

        public void Create(string problemId, string userId, string code)
        {
            var problemTotalPoints = this.db.Problems.Where(x => x.Id == problemId).Select(x => x.Points).FirstOrDefault();

            this.db.Submissions.Add(new Submission
            {
                ProblemId = problemId,
                Code = code,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                AchievedResult = (ushort)this.random.Next(0, problemTotalPoints + 1),
            });

            this.db.SaveChanges();
        }

        public void Delete(string id)
        {
            var submission = this.db.Submissions.FirstOrDefault(x => x.Id == id);
            this.db.Submissions.Remove(submission);
            this.db.SaveChanges();
        }
    }
}
