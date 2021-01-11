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
        public virtual DbSet<Checkin> Checkins { get; set; }
        public virtual DbSet<GateCodes> GateCodes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberStatuses> MemberStatuses { get; set; }
        public virtual DbSet<MemberTypes> MemberTypes { get; set; }
        public virtual DbSet<NoticeQueueItems> NoticeQueueItems { get; set; }
        public virtual DbSet<Notices> Notices { get; set; }
        public virtual DbSet<NoticeStatuses> NoticeStatuses { get; set; }
        public virtual DbSet<NoticeTypes> NoticeTypes { get; set; }
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
                optionsBuilder.UseSqlServer("Server=DESKTOP-6A7TUBO;Database=WLCRaces;uid=sa;password=cleo;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.HasKey(e => e.CheckinId);

                entity.Property(e => e.MemberId).HasColumnName("Member ID");

                entity.Property(e => e.Note)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Checkins)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Checkins__Member__42E1EEFE");
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


            modelBuilder.Entity<MemberStatuses>(entity =>
            {
                entity.HasKey(e => e.MemberStatusId);

                entity.Property(e => e.MemberStatus)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<MemberTypes>(entity =>
            {
                entity.HasKey(e => e.MemberTypeId);

                entity.Property(e => e.MemberTypeId)
                    .HasColumnName("MemberTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Category).HasMaxLength(25);

                entity.Property(e => e.MemberDues).HasColumnType("money");

                entity.Property(e => e.MemberType).HasMaxLength(50);
            });

            modelBuilder.Entity<NoticeQueueItems>(entity =>
            {
                entity.HasKey(e => e.NoticeQueueItemId);


                entity.Property(e => e.DateLastChanged).HasColumnType("smalldatetime");

                entity.Property(e => e.NotificationLocation).HasMaxLength(50);

                entity.HasOne(d => d.Notice)
                    .WithMany(p => p.NoticeQueueItems)
                    .HasForeignKey(d => d.NoticeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NoticeQueueItems_Notices");

                entity.HasOne(d => d.NoticeStatus)
                    .WithMany(p => p.NoticeQueueItems)
                    .HasForeignKey(d => d.NoticeStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NoticeQueueItems_NoticeStatuses");
            });

            modelBuilder.Entity<Notices>(entity =>
            {
                entity.HasKey(e => e.NoticeId);

                entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");

                entity.Property(e => e.DateToSend).HasColumnType("smalldatetime");

                entity.HasOne(d => d.NoticeStatus)
                    .WithMany(p => p.Notices)
                    .HasForeignKey(d => d.NoticeStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notices_NoticeStatuses");

                entity.HasOne(d => d.NoticeType)
                    .WithMany(p => p.Notices)
                    .HasForeignKey(d => d.NoticeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notices_NoticeTypes");
            });

            modelBuilder.Entity<NoticeStatuses>(entity =>
            {
                entity.HasKey(e => e.NoticeStatusId);

                entity.Property(e => e.NoticeStatusId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<NoticeTypes>(entity =>
            {
                entity.HasKey(e => e.NoticeTypeId);

                entity.Property(e => e.NoticeTypeId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20);
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
