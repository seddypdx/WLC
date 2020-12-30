using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WLC.Models
{
    public partial class WLCRacesContext : IdentityDbContext<IdentityUser>
    {
        public WLCRacesContext()
        {
        }

        public WLCRacesContext(DbContextOptions<WLCRacesContext> options)
            : base(options)
        {
           // Database.EnsureCreated();
        }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }


        public virtual DbSet<Cabins> Cabins { get; set; }
        public virtual DbSet<MemberStatuses> MemberStatuses { get; set; }
        public virtual DbSet<Racers> Racers { get; set; }
        public virtual DbSet<Races> Races { get; set; }
        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<RibbonsInStock> RibbonsInStock { get; set; }
        public virtual DbSet<Years> Years { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Checkin> Checkins { get; set; }
        public virtual DbSet<GateCodes> GateCodes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-6A7TUBO;Database=WLCRaces;uid=sa;password=cleo;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            //modelBuilder.Entity<AspNetRoles>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //        .HasMaxLength(128)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetUserClaims>(entity =>
            //{
            //    entity.Property(e => e.UserId)
            //        .IsRequired()
            //        .HasMaxLength(128);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserClaims)
            //        .HasForeignKey(d => d.UserId)
            //        .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            //});

            //modelBuilder.Entity<AspNetUserLogins>(entity =>
            //{
            //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId });

            //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

            //    entity.Property(e => e.ProviderKey).HasMaxLength(128);

            //    entity.Property(e => e.UserId).HasMaxLength(128);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserLogins)
            //        .HasForeignKey(d => d.UserId)
            //        .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            //});

            //modelBuilder.Entity<AspNetUserRoles>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.RoleId });

            //    entity.Property(e => e.UserId).HasMaxLength(128);

            //    entity.Property(e => e.RoleId).HasMaxLength(128);

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.RoleId)
            //        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.UserId)
            //        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            //});

            //modelBuilder.Entity<AspNetUsers>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //        .HasMaxLength(128)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

            //    entity.Property(e => e.UserName)
            //        .IsRequired()
            //        .HasMaxLength(256);
            //});


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

            modelBuilder.Entity<GateCodes>(entity =>
            {
                entity.HasKey(e => e.GatecodeId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberId);
                entity.Property(e => e.CabinId).HasColumnName("Cabin");

            });

            modelBuilder.Entity<Checkin>(entity =>
            {

                entity.HasKey(e => e.CheckinId);

                entity.Property(e => e.MemberId).HasColumnName("Member ID");


            });

            modelBuilder.Entity<Racers>(entity =>
            {
                entity.HasKey(e => e.RacerId);

                entity.Property(e => e.RacerId).HasColumnName("Racer ID");

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

                entity.Property(e => e.ResultId).HasColumnName("Result ID");

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
