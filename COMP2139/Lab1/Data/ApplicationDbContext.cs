using Lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //define one-to-many relationship 
        
        //seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project {ProjectId = 1, Name = "Project 5", Description = "Project 5 description"},
            new Project {ProjectId = 2, Name = "Project 6", Description = "Project 6 description"}
        );

    }
}

