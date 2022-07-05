using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Common;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Social_Network.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _saveUserViewModel;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _saveUserViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public DbSet<User> User { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Post> Posts { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                entry.Entity.DateCreated = DateTime.Now;
                entry.Entity.CreatedBy = _saveUserViewModel.UserName;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Tables
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Friend>().ToTable("Friend");
            modelBuilder.Entity<Commentary>().ToTable("Commentaries");
            modelBuilder.Entity<Post>().ToTable("Posts");
            #endregion

            #region Primary keys
            modelBuilder.Entity<User>().HasKey(h => h.Id);
            modelBuilder.Entity<Friend>().HasKey(h => h.Id);
            modelBuilder.Entity<Commentary>().HasKey(h => h.Id);
            modelBuilder.Entity<Post>().HasKey(h => h.Id);
            #endregion

            #region Relationships

            modelBuilder.Entity<User>()
                .HasMany<Friend>(g => g.Friends)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany<Friend>(g => g.FriendWith)
                .WithOne(s => s.UserFriend)
                .HasForeignKey(g => g.FriendsWith)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany<Post>(s => s.Posts)
                .WithOne(g => g.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany<Commentary>(s => s.Commentaries)
                .WithOne(g => g.Post)
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Configuration

            #region User
            modelBuilder.Entity<User>()
                .Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.UserName)
                .IsRequired();
            #endregion

            #region Friend
            modelBuilder.Entity<Friend>()
                .Property(user => user.UserId)
                .IsRequired();

            modelBuilder.Entity<Friend>()
                .Property(user => user.FriendsWith)
                .IsRequired();
            #endregion

            #region Post
            modelBuilder.Entity<Post>()
                .Property(user => user.PostDescription)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(user => user.UserId)
                .IsRequired();
            #endregion

            #region Commentary
            modelBuilder.Entity<Commentary>()
                .Property(user => user.Description)
                .IsRequired();

            modelBuilder.Entity<Commentary>()
                .Property(user => user.UserId)
                .IsRequired();

            modelBuilder.Entity<Commentary>()
                .Property(user => user.PostId)
                .IsRequired();
            #endregion
            #endregion
        }
    }
}
