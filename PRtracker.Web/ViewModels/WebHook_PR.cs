namespace GitHubPullRequests.ViewModels
{
    public class WebHook_PR
    {
        public string action { get; set; }
        public int number { get; set; }
        public PullRequest pull_request { get; set; }
        public Label label { get; set; }
        public Repo repository { get; set; }
        public User sender { get; set; }
    }
}