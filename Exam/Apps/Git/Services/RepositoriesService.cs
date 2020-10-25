using Git.Data;
using Git.ViewModels.Commits;
using Git.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext db;

        public RepositoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string name, string repositoryType, string userId)
        {
            var repository = new Repository
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                OwnerId = userId,
            };

            if (repositoryType == "Public")
            {
                repository.IsPublic = true;
            }

            this.db.Repositories.Add(repository);
            this.db.SaveChanges();
        }

        public IEnumerable<RepositoryViewModel> GetAll()
        {
            return this.db.Repositories.Where(x=>x.IsPublic).Select(x => new RepositoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Owner = x.Owner.Username,
                CreatedOn = x.CreatedOn.ToString("dd.MM.yyyy HH:mm"),
                CommitsCount = x.Commits.Count,
            }).ToList();
        }

        public CommitInputModel GetById(string id)
        {
            return this.db.Repositories.Where(x => x.Id == id).Select(x => new CommitInputModel
            {
                Name = x.Name,
                Id = x.Id,
            }).FirstOrDefault();
        }
    }
}
