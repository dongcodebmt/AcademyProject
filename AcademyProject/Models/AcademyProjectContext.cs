using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AcademyProject.Models
{
    public partial class AcademyProjectContext : DbContext
    {
        public AcademyProjectContext(DbContextOptions<AcademyProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogComment> BlogComments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Certification> Certifications { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamDetail> ExamDetails { get; set; }
        public virtual DbSet<ExamOption> ExamOptions { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<ExamRightOption> ExamRightOptions { get; set; }
        public virtual DbSet<ExamUser> ExamUsers { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<WillLearn> WillLearns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answers__Questio__1F98B2C1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answers__UserId__208CD6FA");
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.UserId })
                    .HasName("PK__Attendan__1855FD634C672793");

                entity.ToTable("Attendance");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Attendanc__Cours__2FCF1A8A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Attendanc__UserI__30C33EC3");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.PictureId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Blogs__CategoryI__7F2BE32F");

                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.PictureId)
                    .HasConstraintName("FK__Blogs__PictureId__7D439ABD");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Blogs__UserId__7E37BEF6");
            });

            modelBuilder.Entity<BlogComment>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogComments)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BlogComme__BlogI__03F0984C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BlogComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BlogComme__UserI__04E4BC85");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Certification>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId })
                    .HasName("PK__Certific__7B1A1B56DCEAD8AD");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Certifications)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Certifica__Cours__76969D2E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Certifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Certifica__UserI__75A278F5");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.PictureId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Courses__Categor__3C69FB99");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Courses__Lecture__3B75D760");

                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.PictureId)
                    .HasConstraintName("FK__Courses__Picture__3A81B327");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ExamDuration).HasDefaultValueSql("((300))");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exams__CourseId__59FA5E80");
            });

            modelBuilder.Entity<ExamDetail>(entity =>
            {
                entity.HasOne(d => d.ExamUser)
                    .WithMany(p => p.ExamDetails)
                    .HasForeignKey(d => d.ExamUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamDetai__ExamU__6E01572D");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.ExamDetails)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK__ExamDetai__Optio__6FE99F9F");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamDetails)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamDetai__Quest__6EF57B66");
            });

            modelBuilder.Entity<ExamOption>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamOptions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamOptio__Quest__60A75C0F");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamQuest__ExamI__5DCAEF64");
            });

            modelBuilder.Entity<ExamRightOption>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__ExamRigh__0DC06FACF1A12A24");

                entity.Property(e => e.QuestionId).ValueGeneratedNever();

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.ExamRightOptions)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamRight__Optio__6477ECF3");

                entity.HasOne(d => d.Question)
                    .WithOne(p => p.ExamRightOption)
                    .HasForeignKey<ExamRightOption>(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamRight__Quest__6383C8BA");
            });

            modelBuilder.Entity<ExamUser>(entity =>
            {
                entity.Property(e => e.CompletedAt).HasColumnType("datetime");

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamUsers)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamUsers__ExamI__6B24EA82");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExamUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExamUsers__UserI__6A30C649");
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.PicturePath)
                    .IsRequired()
                    .HasMaxLength(2048);
            });

            modelBuilder.Entity<Progress>(entity =>
            {
                entity.HasKey(e => new { e.StepId, e.UserId })
                    .HasName("PK__Progress__F54CBF9363CEEE07");

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Step)
                    .WithMany(p => p.Progresses)
                    .HasForeignKey(d => d.StepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Progresse__StepI__52593CB8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Progresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Progresse__UserI__534D60F1");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.PictureId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Questions__Categ__1AD3FDA4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Questions__UserI__19DFD96B");
            });

            modelBuilder.Entity<Requirement>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Requirements)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Requireme__Cours__46E78A0C");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Step>(entity =>
            {
                entity.Property(e => e.EmbedLink).HasMaxLength(2048);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Steps)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Steps__TrackId__4E88ABD4");
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tracks__CourseId__4AB81AF0");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.PictureId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PictureId)
                    .HasConstraintName("FK__Users__PictureId__2A4B4B5E");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__UserRole__AF2760ADA0C8E124");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserRoles__RoleI__3F115E1A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserRoles__UserI__3E1D39E1");
            });

            modelBuilder.Entity<WillLearn>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.WillLearns)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WillLearn__Cours__440B1D61");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
