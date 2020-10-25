using Git.ViewModels.Commits;
using System.Collections.Generic;

namespace Git.Services
{
    public interface ICommitsService
    {
        void Create(string userId, string repositoryId, string description);

        IEnumerable<CommitViewModel> GetAll(string userId);

        void Delete(string userId, string commitId);
    }
}
