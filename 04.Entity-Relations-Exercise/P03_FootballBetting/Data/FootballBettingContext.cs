namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
            
        }

        public FootballBettingContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureTeamEntity(modelBuilder);

            ConfigureColorEntity(modelBuilder);

            ConfigureTownEntity(modelBuilder);

            ConfigureCountryEntity(modelBuilder);

            ConfigurePlayerEntity(modelBuilder);

            ConfigurePositionEntity(modelBuilder);

            ConfigurePlayerStatisticEntity(modelBuilder);

            ConfigureGameEntity(modelBuilder);

            ConfigureBetEntity(modelBuilder);

            ConfigureUserEntity(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Username)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.Balance)
                    .IsRequired();
            });
        }

        private void ConfigureBetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(b => b.BetId);

                entity.Property(b => b.Amount)
                    .IsRequired();
                
                entity.Property(b => b.Prediction)
                    .IsRequired();
                
                entity.Property(b => b.DateTime)
                    .IsRequired();

                entity
                    .HasOne(b => b.User)
                    .WithMany(u => u.Bets)
                    .HasForeignKey(b => b.UserId);

                entity
                    .HasOne(b => b.Game)
                    .WithMany(g => g.Bets)
                    .HasForeignKey(b => b.GameId);
            });
        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.GameId);

                entity
                    .HasOne(g => g.HomeTeam)
                    .WithMany(t => t.HomeGames)
                    .HasForeignKey(g => g.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(g => g.AwayTeam)
                    .WithMany(t => t.AwayGames)
                    .HasForeignKey(g => g.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(g => g.HomeTeamGoals)
                    .IsRequired();

                entity.Property(g => g.AwayTeamGoals)
                    .IsRequired();

                entity.Property(g => g.DateTime)
                    .IsRequired();

                entity.Property(g => g.HomeTeamBetRate)
                    .IsRequired();

                entity.Property(g => g.AwayTeamBetRate)
                    .IsRequired();

                entity.Property(g => g.DrawBetRate)
                    .IsRequired();

                entity.Property(g => g.Result)
                    .IsRequired();
            });
        }

        private void ConfigurePlayerStatisticEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(ps => new {ps.PlayerId, ps.GameId});

                entity.Property(ps => ps.ScoredGoals)
                    .IsRequired();

                entity.Property(ps => ps.Assists)
                    .IsRequired();

                entity.Property(ps => ps.MinutesPlayed)
                    .IsRequired();

                entity
                    .HasOne(ps => ps.Player)
                    .WithMany(p => p.PlayerStatistics)
                    .HasForeignKey(ps => ps.PlayerId);

                entity
                    .HasOne(ps => ps.Game)
                    .WithMany(g => g.PlayerStatistics)
                    .HasForeignKey(ps => ps.GameId);
            });
        }

        private void ConfigurePositionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(p => p.PositionId);

                entity.Property(p => p.Name)
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }

        private void ConfigurePlayerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.PlayerId);

                entity.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(p => p.SquadNumber)
                    .IsRequired();

                entity.Property(p => p.IsInjured)
                    .IsRequired();

                entity
                    .HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId);

                entity
                    .HasOne(p => p.Position)
                    .WithMany(pos => pos.Players)
                    .HasForeignKey(p => p.PositionId);
            });
        }

        private void ConfigureCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(c => c.CountryId);

                entity.Property(c => c.Name)
                    .HasMaxLength(30)
                    .IsRequired();
            });
        }

        private void ConfigureTownEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>(entity =>
            {
                entity.HasKey(t => t.TownId);

                entity.Property(t => t.Name)
                    .HasMaxLength(30)
                    .IsRequired();

                entity
                    .HasOne(t => t.Country)
                    .WithMany(c => c.Towns)
                    .HasForeignKey(t => t.CountryId);
            });
        }

        private void ConfigureColorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(c => c.ColorId);

                entity.Property(c => c.Name)
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.TeamId);

                entity.Property(t => t.Name)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(t => t.LogoUrl)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.Property(t => t.Initials)
                    .IsRequired()
                    .HasColumnType("CHAR(3)");

                entity.Property(t => t.Budget)
                    .IsRequired();

                entity
                    .HasOne(t => t.PrimaryKitColor)
                    .WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(t => t.SecondaryKitColor)
                    .WithMany(c => c.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(t => t.Town)
                    .WithMany(tn => tn.Teams)
                    .HasForeignKey(t => t.TownId);
            });
        }
    }
}