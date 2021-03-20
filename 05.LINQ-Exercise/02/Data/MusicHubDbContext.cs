namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }
        public DbSet<Writer> Writers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Song>(entity =>
            {
                entity
                .HasOne(a => a.Album)
                .WithMany(s => s.Songs)
                .HasForeignKey(a => a.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

                entity
                .HasOne(w => w.Writer)
                .WithMany(s => s.Songs)
                .HasForeignKey(w => w.WriterId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Album>(entity =>
            {
                entity
                .HasOne(p => p.Producer)
                .WithMany(a => a.Albums)
                .HasForeignKey(p => p.ProducerId);
            });

            builder.Entity<SongPerformer>(entity =>
            {
                entity
                .HasOne(s => s.Song)
                .WithMany(sp => sp.SongPerformers)
                .HasForeignKey(s => s.SongId)
                .OnDelete(DeleteBehavior.NoAction);

                entity
                .HasOne(p => p.Performer)
                .WithMany(ps => ps.PerformerSongs)
                .HasForeignKey(p => p.PerformerId)
                .OnDelete(DeleteBehavior.NoAction);

                entity
                .HasKey(pk => new { pk.SongId, pk.PerformerId });
            });
        }
    }
}
