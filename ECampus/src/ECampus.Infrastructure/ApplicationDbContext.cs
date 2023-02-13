using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public const string ConnectionKey = "ApplicationDbContext";
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public DbSet<Auditory> Auditories { get; set; } = default!;
    public DbSet<Class> Classes { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<Faculty> Faculties { get; set; } = default!;
    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<Teacher> Teachers { get; set; } = default!;
    public DbSet<Subject> Subjects { get; set; } = default!;
    public DbSet<SubjectTeacher> SubjectTeachers { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserAuditory> UserAuditories { get; set; } = default!;
    public DbSet<UserGroup> UserGroups { get; set; } = default!;
    public DbSet<UserTeacher> UserTeachers { get; set; } = default!;

    public DbSet<Student> Students { get; set; } = default!;

    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<CourseTask> CourseTasks { get; set; } = default!;
    public DbSet<TaskSubmission> TaskSubmissions { get; set; } = default!;
    public DbSet<CourseGroup> CourseGroups { get; set; } = default!;
    public DbSet<CourseTeacher> CourseTeachers { get; set; } = default!;

    public DbSet<TeacherRate> TeacherRates { get; set; } = default!;

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.UpdateDeleteStatus();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.UpdateDeleteStatus();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}