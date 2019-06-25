using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WLC.Models
{
    public partial class WLCRacesContext : DbContext
    {
        public WLCRacesContext()
        {
        }

        public WLCRacesContext(DbContextOptions<WLCRacesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cabins> Cabins { get; set; }
        public virtual DbSet<MemberStatuses> MemberStatuses { get; set; }
        public virtual DbSet<Racers> Racers { get; set; }
        public virtual DbSet<Races> Races { get; set; }
        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<RibbonsInStock> RibbonsInStock { get; set; }
        public virtual DbSet<Years> Years { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(local);Database=WLCRaces;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Cabins>(entity =>
            {
                entity.HasKey(e => e.CabinId);

                entity.Property(e => e.CabinId)
                    .HasColumnName("Cabin ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CabinName)
                    .HasColumnName("Cabin Name")
                    .HasMaxLength(30);

                entity.Property(e => e.CabinPhone)
                    .HasColumnName("Cabin Phone")
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<MemberStatuses>(entity =>
            {
                entity.HasKey(e => e.MemberStatusId);

                entity.Property(e => e.MemberStatus)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Racers>(entity =>
            {
                entity.HasKey(e => e.RacerId);

                entity.Property(e => e.RacerId)
                    .HasColumnName("Racer ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.BoyOrGirl)
                    .HasColumnName("Boy or Girl")
                    .HasMaxLength(255);

                entity.Property(e => e.CabinId).HasColumnName("Cabin ID");

                entity.Property(e => e.FirstName)
                    .HasColumnName("First Name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .HasColumnName("Last Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Cabin)
                    .WithMany(p => p.Racers)
                    .HasForeignKey(d => d.CabinId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Racers_cabins");

                entity.HasOne(d => d.MemberStatus)
                    .WithMany(p => p.Racers)
                    .HasForeignKey(d => d.MemberStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Racers_MemberStatus");
            });

            modelBuilder.Entity<Races>(entity =>
            {
                entity.HasKey(e => e.RaceId);

                entity.Property(e => e.RaceId)
                    .HasColumnName("Race ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsBoating).HasColumnName("isBoating");

                entity.Property(e => e.MaximumAge).HasColumnName("Maximum Age");

                entity.Property(e => e.MinimumAge).HasColumnName("Minimum Age");

                entity.Property(e => e.RaceBoyOrGirl)
                    .HasColumnName("Race Boy or Girl")
                    .HasMaxLength(255);

                entity.Property(e => e.RaceName)
                    .HasColumnName("Race Name")
                    .HasMaxLength(255);

                entity.Property(e => e.RaceOrder).HasColumnName("race order");

                entity.Property(e => e.RacePoints)
                    .HasColumnName("Race Points")
                    .HasMaxLength(50);

                entity.Property(e => e.SortOrder).HasColumnName("sort order");
            });

            modelBuilder.Entity<Results>(entity =>
            {
                entity.HasKey(e => e.ResultId);

                entity.Property(e => e.ResultId)
                    .HasColumnName("Result ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comments).HasMaxLength(50);

                entity.Property(e => e.PointsPlace).HasColumnName("Points Place");

                entity.Property(e => e.RaceId).HasColumnName("Race ID");

                entity.Property(e => e.RacerId).HasColumnName("Racer ID");

                entity.Property(e => e.Ribbon).HasMaxLength(30);

                entity.Property(e => e.TeamId).HasColumnName("Team ID");

                entity.Property(e => e.Year).HasDefaultValueSql("((2004))");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.RaceId)
                    .HasConstraintName("FK_Results_Race");

                entity.HasOne(d => d.Racer)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.RacerId)
                    .HasConstraintName("FK_Results_Racer");

                entity.HasOne(d => d.YearNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.Year)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_Year");
            });

            modelBuilder.Entity<RibbonsInStock>(entity =>
            {
                entity.HasKey(e => e.RibonId);

                entity.ToTable("Ribbons_in_Stock");

                entity.Property(e => e.RibonId).ValueGeneratedNever();

                entity.Property(e => e.NumberInStock).HasColumnName("Number_in_stock");

                entity.Property(e => e.Ribbon).HasMaxLength(50);
            });

            modelBuilder.Entity<Years>(entity =>
            {
                entity.HasKey(e => e.Year);

                entity.Property(e => e.Year).ValueGeneratedNever();

                entity.Property(e => e.LaborDayDate).HasColumnType("datetime");
            });
        }
    }
}
