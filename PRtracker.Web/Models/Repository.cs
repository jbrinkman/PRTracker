using System.Collections.Generic;

namespace PRTracker.Web.Models
{
    public class Repository
    {
        public Repository()
        {
            Notifications = new List<Notification>();
            RepositoryHistories = new List<RepositoryHistory>();
            PullRequests = new List<PullRequest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Notification> Notifications { get; private set; } 
        public virtual List<RepositoryHistory> RepositoryHistories { get; private set; }
        public virtual List<PullRequest> PullRequests { get; private set; }
    }
}