using System;

namespace PRTracker.Web.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string RawJson { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int? RepositoryId { get; set; }
        public bool? Processed { get; set; }

        public virtual Repository Repository { get; set; }
    }
}