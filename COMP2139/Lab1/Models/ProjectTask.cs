using System.ComponentModel.DataAnnotations;
namespace Lab1.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required]
    public required string Title {get; set;}
    
    [Required]
    public required string Description {get; set;}
    
    // Foreign Key
    public int ProjectId { get; set; }
    
    //Navigation Property
    // This property allows for easy access to the related Project entity from the ProjectTask entity
    public Project? Project { get; set; }
    
    
}