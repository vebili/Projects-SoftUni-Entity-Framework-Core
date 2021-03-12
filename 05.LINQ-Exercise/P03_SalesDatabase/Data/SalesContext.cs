namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureProductEntity(modelBuilder);

            ConfigureCustomerEntity(modelBuilder);

            ConfigureStoreEntity(modelBuilder);

            ConfigureSaleEntity(modelBuilder);
        }

        private void ConfigureSaleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity
                    .HasKey(s => s.SaleId);

                entity.Property(s => s.Date)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity
                    .HasOne(s => s.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(s => s.ProductId);

                entity
                    .HasOne(s => s.Customer)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(s => s.CustomerId);

                entity
                    .HasOne(s => s.Store)
                    .WithMany(st => st.Sales)
                    .HasForeignKey(s => s.StoreId);
            });
        }

        private void ConfigureStoreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>(entity =>
            {
                entity
                    .HasKey(s => s.StoreId);

                entity.Property(s => s.Name)
                    .HasMaxLength(80)
                    .IsUnicode()
                    .IsRequired();

                entity
                    .HasMany(st => st.Sales)
                    .WithOne(s => s.Store);
            });
        }

        private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity
                    .HasKey(c => c.CustomerId);

                entity.Property(c => c.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(c => c.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(c => c.CreditCardNumber)
                    .IsRequired();

                entity
                    .HasMany(c => c.Sales)
                    .WithOne(s => s.Customer);
            });
        }

        private void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                    .HasKey(p => p.ProductId);

                entity.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(p => p.Quantity)
                    .IsRequired();

                entity.Property(p => p.Price)
                    .IsRequired();

                entity.Property(p => p.Description)
                    .HasMaxLength(250)
                    .IsUnicode()
                    .HasDefaultValue("No description");

                entity
                    .HasMany(p => p.Sales)
                    .WithOne(s => s.Product);
            });
        }
    }
}