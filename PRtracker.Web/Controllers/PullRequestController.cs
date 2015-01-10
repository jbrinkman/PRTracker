using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GitHubPullRequests.Components;
using GitHubPullRequests.Data;
using GitHubPullRequests.Models;
using GitHubPullRequests.ViewModels;
using PullRequest = GitHubPullRequests.Models.PullRequest;
using User = GitHubPullRequests.Models.User;

namespace GitHubPullRequests.Controllers
{
    public class PullRequestController : ApiController
    {
        // GET: api/PullRequest
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PullRequest/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PullRequest
        [GitHubAuthorizationFilter]
        public async void Post([FromBody]ViewModels.WebHook_PR pr)
        {
            var body = await this.ActionContext.Request.Content.ReadAsStringAsync();

            using (var db = new PullRequestContext())
            {
                var creator = GetUser(db, pr.pull_request.user);
                var repo = GetRepo(db, pr.repository);

                SavePullRequest(db, pr.pull_request, repo, creator);
                SaveNotification(db, repo, body);
            }
        }

        private static void SaveNotification(PullRequestContext db, Repository repo, string body)
        {
            var notification = new Notification
            {
                RawJson = body,
                ReceivedDate = DateTime.UtcNow,
                Repository = repo,
                RepositoryId = repo.Id
            };

            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        private static void SavePullRequest(PullRequestContext db, ViewModels.PullRequest pullRequest, Repository repo, User creator)
        {
            var pr = db.PullRequests.Find(pullRequest.id);
            if (pr != null) return ;

            pr = new PullRequest
            {
                Id = pullRequest.id,
                Repository = repo,
                RepositoryId = repo.Id,
                CreatedByUser = creator,
                Number = pullRequest.number
            };
            db.PullRequests.Add(pr);
            db.SaveChanges();
        }

        private static Repository GetRepo(PullRequestContext db, Repo repository)
        {
            var repo = db.Repositories.Find(repository.id) ?? new Repository
            {
                Id = repository.id,
                Name = repository.full_name,
                Url = repository.html_url
            };
            return repo;
        }

        private static User GetUser(PullRequestContext db, ViewModels.User vmUser)
        {
            var user = db.Users.Find(vmUser.id) ?? new User
            {
                Id = vmUser.id,
                Login = vmUser.login
            };
            return user;
        }
    }
}
