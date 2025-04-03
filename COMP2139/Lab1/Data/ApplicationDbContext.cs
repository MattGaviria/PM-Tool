using Lab1.Areas.ProjectManagement.Models;
using Lab1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    
    public DbSet<ProjectComment> ProjectComments { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Ensure Identity Configurations and Table are created 
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("Identity");
        
        //define one-to-many relationship 
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)        // one project has (potentioally) many tasks
            .WithOne(t => t.Project)            // Each ProjectTask belongs to one Project
            .HasForeignKey(t => t.ProjectId)  // Foreign key in projectTask table
            .OnDelete(DeleteBehavior.Cascade);          // Cascade Delete ProjectTask when a Project is deleted 
        //seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project {ProjectId = 1, Name = "Project 5", Description = "Project 5 description"},
            new Project {ProjectId = 2, Name = "Project 6", Description = "Project 6 description"},
            new Project {ProjectId = 3, Name = "Project 7", Description = "Project 7 description"},
            new Project {ProjectId = 4, Name = "Project 8", Description = "Project 8 description"},
            new Project {ProjectId = 5, Name = "Project 9", Description = "Project 9 description"},
            new Project {ProjectId = 6, Name = "Project 10", Description = "Project 10 description"},
            new Project {ProjectId = 7, Name = "Project 11", Description = "Project 11 description"},
            new Project {ProjectId = 8, Name = "Project 12", Description = "Project 12 description"}
        );

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("User");
               
        });
        
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("Role");
               
        });
        
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
               
        });
        
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
               
        });
        
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
               
        });
        
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
               
        });
        
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
               
        });
        

    }
}

