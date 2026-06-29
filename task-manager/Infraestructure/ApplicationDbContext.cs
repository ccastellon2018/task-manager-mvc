using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task_manager.Entities;

namespace task_manager.Infraestructure;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.TaskId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Order).IsRequired();
            entity.Property(e => e.CreateAt).IsRequired();
        });

        modelBuilder.Entity<StepEntity>(entity =>
        {
            entity.HasKey(e => e.StepId);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Done).IsRequired();
            entity.Property(e => e.StepOrder).IsRequired();
            entity.HasOne(e => e.Task_Entity)
                .WithMany(t => t.Steps)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AttachmentEntity>(entity =>
        {
            entity.HasKey(e => e.AttachmentId);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500).IsUnicode(false);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.AttachmentOrder).IsRequired();
            entity.HasOne(e => e.Task_Entity)
                .WithMany(t => t.Attachments)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<StepEntity> Steps { get; set; }
    public DbSet<AttachmentEntity> Attachments { get; set; }
}
