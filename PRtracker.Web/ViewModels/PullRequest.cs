using System;

namespace PRTracker.Web.ViewModels
{
    public class PullRequest
    {
        public string url { get; set; }
        public int id { get; set; }
        public string html_url { get; set; }
        public string diff_url { get; set; }
        public string patch_url { get; set; }
        public string issue_url { get; set; }
        public int number { get; set; }
        public string state { get; set; }
        public bool locked { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? closed_at { get; set; }
        public DateTime? merged_at { get; set; }
        public string merge_commit_sha { get; set; }
        public User assignee { get; set; }
        public object milestone { get; set; }
        public string commits_url { get; set; }
        public string review_comments_url { get; set; }
        public string review_comment_url { get; set; }
        public string comments_url { get; set; }
        public string statuses_url { get; set; }
        public CompareRepo head { get; set; }
        public CompareRepo @base { get; set; }
        public Links _links { get; set; }
        public bool merged { get; set; }
        public bool mergeable { get; set; }
        public string mergeable_state { get; set; }
        public User merged_by { get; set; }
        public int comments { get; set; }
        public int review_comments { get; set; }
        public int commits { get; set; }
        public int additions { get; set; }
        public int deletions { get; set; }
        public int changed_files { get; set; }
    }
}