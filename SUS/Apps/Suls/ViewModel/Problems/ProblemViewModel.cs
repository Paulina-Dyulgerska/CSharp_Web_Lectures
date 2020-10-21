using System.Collections.Generic;

namespace Suls.ViewModel.Problems
{
   public class ProblemViewModel
    {
        public ProblemViewModel()
        {
            this.Submissions = new List<SubmissionViewModel>();
        }

        public string  Name { get; set; }

        public IEnumerable<SubmissionViewModel> Submissions { get; set; }
    }
}
