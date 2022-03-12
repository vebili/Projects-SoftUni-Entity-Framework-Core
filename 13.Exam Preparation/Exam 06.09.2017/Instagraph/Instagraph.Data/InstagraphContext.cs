using Instagraph.Models;
using Microsoft.EntityFrameworkCore;

namespace Instagraph.Data
{
    public class InstagraphContext : DbContext
    {
        public InstagraphContext() { }

        public InstagraphContext(DbContextOptions options)
            :base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserFollower> UsersFollowers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Picture
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Path)
                .IsRequired(true);
            });

            //User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsRequired(true);

                entity.HasIndex(e => e.Username)
                    .IsUnique(true);

                entity.Property(e => e.Password)
                    .HasMaxLength(20);

                entity.HasOne(u => u.ProfilePicture)
                    .WithMany(p => p.Users)
                    .HasForeignKey(u => u.ProfilePictureId);
            });

            //Post
            modelBuilder.Entity<Post>(entity => 
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Caption)
                    .IsRequired(true);

                entity.HasOne(p => p.Picture)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(p => p.PictureId);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                    .HasMaxLength(250)
                    .IsRequired(true);

                entity.HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(c => c.PostId);

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //UserFollower
            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.UserId,
                    e.FollowerId
                });

                entity.HasOne(uf => uf.User)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(uf => uf.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uf => uf.Follower)
                    .WithMany(u => u.UsersFollowing)
                    .HasForeignKey(uf => uf.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
