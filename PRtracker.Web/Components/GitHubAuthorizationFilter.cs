using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace GitHubPullRequests.Components
{
    /// <summary>
    /// Authorization filter to check that the GitHub provided signature is valid for the specified content.
    /// </summary>
    /// <remarks>
    /// GitHub webhooks will pass a signature in the X-Hub-Signature header which is a SHA1 digest of the body contents.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class GitHubAuthorizationFilter : AuthorizationFilterAttribute
    {

        public bool Active { get; private set; }
        public string Secret { get; private set; }

        public GitHubAuthorizationFilter() : this(ConfigurationManager.AppSettings["webhook-secret"], true)
        { }

        /// <summary>
        /// The GitHub Authorization filter requires a key used for signing the body of the webhook post.
        /// </summary>
        /// <param name="secret"></param>
        public GitHubAuthorizationFilter(string secret) : this(secret, true)
        { }

        /// <summary>
        /// Overriden constructor to allow explicit disabling of this
        /// filter's behavior. Pass false to disable (same as no filter
        /// but declarative)
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="active"></param>
        public GitHubAuthorizationFilter(string secret, bool active)
        {
            Secret = secret;
            Active = active;
        }

        /// <summary>
        /// Adds a filter to verify that a valid X-Hub-Signature exists in the request header
        /// </summary>
        /// <param name="actionContext"></param>
        public override async void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (Active)
            {
                if (actionContext.Request.Headers.Contains("X-Hub-Signature"))
                {
                    var signature = actionContext.Request.Headers.GetValues("X-Hub-Signature").FirstOrDefault().Replace("sha1=", "");
                    var body = await actionContext.Request.Content.ReadAsStringAsync();

                    var token = CreateToken(body, Secret);

                    if (token == signature)
                    {
                        return;
                    }
                }

                // We did not have a valid signature
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            byte[] keyByte = Encoding.ASCII.GetBytes(secret);
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
    }
}