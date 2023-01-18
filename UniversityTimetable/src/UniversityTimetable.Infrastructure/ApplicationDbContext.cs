using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure;

public class ApplicationDbContext : DbContext
{
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