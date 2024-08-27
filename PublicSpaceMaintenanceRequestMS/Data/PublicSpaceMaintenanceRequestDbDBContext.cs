using Microsoft.EntityFrameworkCore;

namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class PublicSpaceMaintenanceRequestDbDBContext : DbContext
    {
        public PublicSpaceMaintenanceRequestDbDBContext(DbContextOptions options) : base(options)
        {
        }

        protected PublicSpaceMaintenanceRequestDbDBContext()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Officer> Officers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Pending> Pendings { get; set; }
        public virtual DbSet<InProgress> InProgresses { get; set; }
        public virtual DbSet<Complete> Completes { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Username, "UQ_USERNAME").IsUnique();
                entity.HasIndex(e => e.Lastname, "IX_LASTNAME");
                entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();

                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(60);
                entity.Property(e => e.Firstname).HasMaxLength(50);
                entity.Property(e => e.Lastname).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Phonenumber).HasMaxLength(50);
                entity.Property(e => e.Role).HasConversion<string>().HasMaxLength(20);
            });

            modelBuilder.Entity<Citizen>(entity =>
            {
                entity.ToTable("Citizens");

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                    .WithOne(e => e.Citizen)
                    .HasForeignKey<Citizen>(e => e.UserId)
                    .HasConstraintName("FK_Citizens_Users");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admins");

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(p => p.UserId)
                    .HasConstraintName("FK_Admins_Users");
            });

            modelBuilder.Entity<Officer>(entity =>
            {
                entity.ToTable("Officers");

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Department)
                    .WithMany(p => p.Officers)
                    .HasForeignKey(p => p.DepartmentId);

                entity.HasOne(e => e.User)
                    .WithOne(e => e.Officer)
                    .HasForeignKey<Officer>(e => e.UserId)
                    .HasConstraintName("FK_Officers_Users");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Title, "UQ_Name").IsUnique();
                
                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Requests");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Status, "IX_Status");
                entity.HasIndex(e => e.CreateDate, "IX_CreateDate");
                
                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasColumnName("RequestStatus")
                    .HasConversion<string>()
                    .HasMaxLength(50);
                entity.Property(e => e.CreateDate).HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Citizen)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(p => p.CitizenId)
                    .HasConstraintName("FK_Requests_Citizens");

                entity.HasOne(e => e.AssignedDepartment)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(p => p.AssignedDepartmentId)
                    .HasConstraintName("FK_Requests_Departments");
            });
        }
    }
}
