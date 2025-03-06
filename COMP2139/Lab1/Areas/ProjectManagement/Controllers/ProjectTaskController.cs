using Lab1.Areas.ProjectManagement.Models;
using Lab1.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("ProjectTask")]

public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;

    }

    [HttpGet("Index/{projectId:int}")]

    public IActionResult Index(int projectId)
    {
        var tasks = _context
            .Tasks
            .Where(t => t.ProjectId == projectId)
            .ToList();

        ViewBag.ProjectId = projectId;
        return View(tasks);
    }

    [HttpGet("Details/{id:int}")]
    public IActionResult Details(int id)
    {
        var task = _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    [HttpGet("Create/{projectId:int}")]
    public IActionResult Create(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project == null)
        {
            return NotFound();
        }

        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = "",
        };

        return View();
    }

    [HttpPost("Create/{projectId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });

        }

        return View(task);

    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var task = _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);

    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });

        }
        return View(task);
    }

    [HttpGet("Delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var task = _context
            .Tasks
            .Include(t=>t.Project)
            .FirstOrDefault(t=>t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost("Delete/{projectTaskId}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int projectTaskId)
    {
        var task = _context.Tasks.Find(projectTaskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new {projectId = task.ProjectId});
        }

        return View(task);
    }

    
    [HttpGet("Search")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        var tasksQuery =_context.Tasks.AsQueryable();
        
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (projectId.HasValue)
        {
            tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);
        }
        

        if (searchPerformed)
        {
            searchString = searchString.ToLower();
            
            tasksQuery = tasksQuery.Where(t => t.Title.ToLower().Contains(searchString) ||
                                               t.Description.ToLower().Contains(searchString));
            
        }
        
        // Asynchronus execution means this method does not block the thread while waiting for the database 
        var projects = await tasksQuery.ToListAsync();
        
        //Pass search metedata
        ViewBag.ProjectId = projectId;
        ViewData["SearchString"] = searchString;
        ViewData["SearchPerformed"] = searchPerformed;
        
        
        return View("Index", projects);
    }


}