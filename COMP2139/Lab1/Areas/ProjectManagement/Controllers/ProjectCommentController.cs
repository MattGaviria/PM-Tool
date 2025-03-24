using Lab1.Areas.ProjectManagement.Models;
using Lab1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Areas.ProjectManagement.Controllers;


[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectCommentController: Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectCommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int projectId)
    {
        var comments = await _context.ProjectComments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToListAsync();
        
        return Json(comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
    {
        if (ModelState.IsValid)
        {
            //Current date and time of the comment
            comment.DatePosted = DateTime.Now;
            
            // Save the comment
            _context.ProjectComments.Add(comment);
            
            //Commit do the database
            await _context.SaveChangesAsync();
            
            return Json(new { success = true, message = "Comment successfully added." });
        }
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        return Json(new { success = false, message = "Comment could not be added.", errors = errors });
    }
}