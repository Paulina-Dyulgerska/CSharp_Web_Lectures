using Suls.ViewModel.Problems;
using System.Collections.Generic;

namespace Suls.Services
{
    public interface IProblemsService
    {
        void Create(string name, ushort points);

        IEnumerable<HomePageProblemViewModel> GetAll();

        string GetNameById(string id);

        ProblemViewModel GetAllByProblemId(string userId);
    }
}
