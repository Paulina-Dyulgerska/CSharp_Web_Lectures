using System;

namespace Suls.ViewModel.Problems
{
    public class SubmissionViewModel
    {
        public string Username { get; set; }

        public string SubmissionId { get; set; }

        public int AchievedResult { get; set; }

        public int MaxPoints { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Percentage => (int)Math.Round(this.AchievedResult * 100M / this.MaxPoints);
    }
}
