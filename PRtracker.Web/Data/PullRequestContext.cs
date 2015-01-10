using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GitHubPullRequests.Models;

namespace GitHubPullRequests.Data
{
    public class PullRequestContext : DbContext
    {
        public PullRequestContext() : base("PullRequestDB")
        { }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<PullRequest> PullRequests { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repository>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<PullRequest>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }
    }
}

// .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)