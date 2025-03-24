using System.ComponentModel.DataAnnotations;

namespace Lab1.Areas.ProjectManagement.Models;

public class ProjectComment
{
    public int ProjectCommentId { get; set; }
    
    [Display(Name="Project Message")]
    [StringLength(500, ErrorMessage = "Project Message length cannot be longer than 500 characters.")]
    public string? Content { get; set; }

    
    [Display(Name="Date Posted")]
    [DataType(DataType.DateTime)]
    private DateTime _datePosted;
    public DateTime DatePosted
    {
        get => _datePosted;
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    // foregn 
    public int ProjectId { get; set; }
    
    // Navigation property
    public Project? Project { get; set; }
}