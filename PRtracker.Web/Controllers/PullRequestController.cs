using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using log4net;
using PRTracker.Web.Components;
using PRTracker.Web.Data;
using PRTracker.Web.Models;
using PRTracker.Web.ViewModels;
using PullRequest = PRTracker.Web.Models.PullRequest;
using User = PRTracker.Web.Models.User;

namespace PRTracker.Web.Controllers
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
        public void Post([NakedBody] string body) //[FromBody]ViewModels.WebHook_PR pr)
        {
            try
            {
                var pr = body.FromJson<ViewModels.WebHook_PR>();

                using (var db = new PullRequestContext())
                {
                    var sender = GetUser(db, pr.sender);
                    var creator = GetUser(db, pr.pull_request.user);
                    var repo = GetRepo(db, pr.repository);
                    var pullRequest = pr.pull_request;

                    if (pr.action == "labeled" && pr.label.name == "merged")
                    {
                        pullRequest.merged = true;
                        pullRequest.merged_at = pullRequest.closed_at;
                    }

                    SavePullRequest(db, pullRequest, repo, creator, sender);
                    SaveNotification(db, repo, body);
                }

            }
            catch (Exception exception)
            {
                var log = LogManager.GetLogger("PRTracker");
                log.Error(exception);
                throw;
            }
        }

        private static void SaveNotification(PullRequestContext db, Repository repo, string body)
        {
            var notification = new Notification
            {
                RawJson = body,
                ReceivedDate = DateTime.UtcNow,
                Repository = repo,
                RepositoryId = repo.Id,
                Processed = true
            };

            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        private static void SavePullRequest(PullRequestContext db, ViewModels.PullRequest pullRequest, Repository repo, User creator, User sender)
        {
            var pr = db.PullRequests.Find(pullRequest.id);
            if (pr != null)
            {
                if (pr.CreatedDate == null)
                {
                    pr.CreatedDate = pullRequest.created_at;
                    pr.CreatedByUserId = creator.Id;
                    pr.CreatedByUser = creator;
                }
                if (pr.ClosedDate == null && pullRequest.closed_at != null)
                {
                    pr.Closed = true;
                    pr.ClosedDate = pullRequest.closed_at;
                }
                if (!pr.Merged && pullRequest.merged)
                {
                    pr.Merged = pullRequest.merged;
                    pr.MergedDate = pullRequest.merged_at;
                    pr.MergedByUser = sender;
                    pr.MergedByUserId = sender.Id;
                }
            }
            else
            {
                pr = new PullRequest
                {
                    Id = pullRequest.id,
                    Repository = repo,
                    RepositoryId = repo.Id,
                    CreatedByUser = creator,
                    CreatedDate = pullRequest.created_at,
                    Number = pullRequest.number
                };
                if (pullRequest.merged)
                {
                    pr.Merged = pullRequest.merged;
                    pr.MergedDate = pullRequest.merged_at;
                    pr.MergedByUser = sender;
                    pr.MergedByUserId = sender.Id;
                }
                    db.PullRequests.Add(pr);
            }
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
