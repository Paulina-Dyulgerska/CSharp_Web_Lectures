using Git.Data;
using Git.ViewModels.Commits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class CommitsService : ICommitsService
    {
        private readonly ApplicationDbContext db;
        public CommitsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string userId, string repositoryId, string description)
        {
            var commit = new Commit
            {
                CreatorId = userId,
                RepositoryId = repositoryId,
                Description = description,
                CreatedOn = DateTime.UtcNow,
            };

            this.db.Commits.Add(commit);
            this.db.SaveChanges();
        }

        public void Delete(string userId, string commitId)
        {
            var commit = this.db.Commits.FirstOrDefault(x => x.Id == commitId && x.CreatorId == userId);
            this.db.Commits.Remove(commit);
            this.db.SaveChanges();
        }

        public IEnumerable<CommitViewModel> GetAll(string userId)
        {
            return this.db.Commits.Where(x => x.CreatorId == userId)
                .Select(x => new CommitViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn.ToString("dd.MM.yyyy HH:mm"),
                    Description = x.Description,
                    Repository = x.Repository.Name,
                    RepositoryId = x.RepositoryId,
                }).ToList();
        }
    }
}
