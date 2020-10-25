using Git.ViewModels.Commits;
using Git.ViewModels.Repositories;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IRepositoriesService
    {
        void Create(string name, string repositoryType, string userId);

        CommitInputModel GetById(string id);

        IEnumerable<RepositoryViewModel> GetAll();
    }
}
