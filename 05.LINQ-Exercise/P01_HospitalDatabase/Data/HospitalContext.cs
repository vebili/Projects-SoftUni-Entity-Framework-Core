namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurePatientEntity(modelBuilder);

            ConfigureVisitationEntity(modelBuilder);

            ConfigureDiagnoseEntity(modelBuilder);

            ConfigureMedicamentEntity(modelBuilder);

            ConfigurePatientMedicamentEntity(modelBuilder);

            ConfigureDoctorEntity(modelBuilder);
        }

        private void ConfigureDoctorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.DoctorId); 

                entity.Property(d => d.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(d => d.Specialty)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();
            });
        }

        private void ConfigurePatientMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity
                    .HasKey(pm => new {pm.PatientId, pm.MedicamentId});

                entity
                    .HasOne(pm => pm.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(pm => pm.PatientId);

                entity
                    .HasOne(pm => pm.Medicament)
                    .WithMany(m => m.Prescriptions)
                    .HasForeignKey(pm => pm.MedicamentId);
            });
        }

        private void ConfigureMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(m => m.MedicamentId); 

                entity.Property(m => m.Name)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();
            });
        }

        private void ConfigureDiagnoseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(d => d.DiagnoseId); 

                entity.Property(d => d.Name)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(d => d.Comments)
                    .HasMaxLength(250)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(d => d.PatientId)
                    .IsRequired();

                entity
                    .HasOne(d => d.Patient)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.PatientId);

            });
        }

        private void ConfigureVisitationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.HasKey(v => v.VisitationId);

                entity.Property(v => v.Date)
                    .IsRequired();

                entity.Property(v => v.Comments)
                    .HasMaxLength(250)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(v => v.PatientId)
                    .IsRequired();

                entity
                    .HasOne(v => v.Patient)
                    .WithMany(p => p.Visitations)
                    .HasForeignKey(v => v.PatientId);

                entity.Property(v => v.DoctorId)
                    .IsRequired();

                entity
                    .HasOne(v => v.Doctor)
                    .WithMany(d => d.Visitations)
                    .HasForeignKey(v => v.DoctorId);
            });
        }

        private static ModelBuilder ConfigurePatientEntity(ModelBuilder modelBuilder)
        {
            return modelBuilder.Entity<Patient>(entity =>
            {
                entity
                    .HasKey(p => p.PatientId);

                entity
                    .HasMany(p => p.Visitations)
                    .WithOne(v => v.Patient);

                entity
                    .HasMany(p => p.Diagnoses)
                    .WithOne(d => d.Patient);

                entity.Property(p => p.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(p => p.LastName)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(p => p.Address)
                    .HasMaxLength(250)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(p => p.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(p => p.HasInsurance)
                    .IsRequired();
            });
        }
    }
}