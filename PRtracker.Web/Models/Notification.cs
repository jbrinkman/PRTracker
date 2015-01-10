using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GitHubPullRequests.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string RawJson { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int? RepositoryId { get; set; }

        public virtual Repository Repository { get; set; }
    }

    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Notification> Notifications { get; private set; } = new List<Notification>();
        public virtual List<RepositoryHistory> RepositoryHistories { get; private set; } = new List<RepositoryHistory>();
        public virtual List<PullRequest> PullRequests { get; private set; } = new List<PullRequest>();
    }

    public class RepositoryHistory
    {
        public int Id { get; set; }
        public int Forks { get; set; }
        public int Watchers { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RepositoryId { get; set; }

        public virtual Repository Repository { get; set; }
    }

    public class PullRequest
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
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

    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string DnnLogin { get; set; }
        public string Name { get; set; } 
    }

}