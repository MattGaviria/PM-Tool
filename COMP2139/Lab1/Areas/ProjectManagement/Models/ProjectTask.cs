using System.ComponentModel.DataAnnotations;


namespace Lab1.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    
    [Display(Name = "Task Title")]
    [StringLength(100, ErrorMessage = "Task Title cannot be longer than 100 characters.")]
    [Required]
    public required string Title {get; set;}
    
    [Display(Name = "Task Description")]
    [StringLength(500, ErrorMessage = "Task Description cannot be longer than 500 characters.")]
    [Required]
    public required string Description {get; set;}
    
    // Foreign Key
    [Display(Name = "Parent Project ID")]
    public int ProjectId { get; set; }
    
    //Navigation Property
    // This property allows for easy access to the related Project entity from the ProjectTask entity
    public Project? Project { get; set; }
    
   
    
}