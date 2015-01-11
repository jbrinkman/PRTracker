using System;

namespace PRTracker.Web.Models
{
    public class RepositoryHistory
    {
        public int Id { get; set; }
        public int Forks { get; set; }
        public int Watchers { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RepositoryId { get; set; }

        public virtual Repository Repository { get; set; }
    }
}