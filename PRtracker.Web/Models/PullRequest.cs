using System;

namespace PRTracker.Web.Models
{
    public class PullRequest
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByUserId { get; set; }
        public int Number { get; set; }
        public bool Closed { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool Merged { get; set; }
        public DateTime? MergedDate { get; set; }
        public int? MergedByUserId { get; set; }

        public virtual Repository Repository { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User MergedByUser { get; set; }
        public string Milestone { get; set; }

    }
}