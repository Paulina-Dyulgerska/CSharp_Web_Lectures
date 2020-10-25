using Git.ViewModels.Commits;
using Git.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Services
{
    public interface IRepositoriesService
    {
        void Create(string name, string repositoryType, string userId);

        CommitInputModel GetById(string id);


        IEnumerable<RepositoryViewModel> GetAll();
    }
}
