using Lab1.Areas.ProjectManagement.Models;
using Lab1.Controllers;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Lab1.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectController : Controller
{
    private readonly ILogger<ProjectController> _logger;
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context, ILogger<ProjectController> logger)
    {
        _logger = logger;
        _context = context;
    }
    /// <summary>
    /// Index action will retrieve a listing of projects (database)
    /// </summary>
    
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Accessed ProjectController Index at {Time}", DateTime.Now);
        // Retrieve all projects from the database
        var projects=await _context.Projects.ToListAsync();
        return View(projects);
        
       // var projects = new List<Project>
        //{
         //   new Project { ProjectId = 1, Name = "Project1", Description = " First Project 1" },
       // };
        //return View(projects);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        _logger.LogInformation("Accessed ProjectController Create at {Time}", DateTime.Now);
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        //Persist new project to the database
        if (ModelState.IsValid)
        {
            project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
            project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);
            
            _context.Projects.Add(project); // add new project to database
            await _context.SaveChangesAsync(); //persists the changes to database
            return RedirectToAction("Index");
        }
        
        return View("Index");
    }
    
    //CRUD CREATE - READ -UPDATE - DELETE
    
    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        _logger.LogInformation("Accessed ProjectController Details at {Time}", DateTime.Now);
        // retrieves the project with the specified id or returns null if not found
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            _logger.LogWarning("Could not find the Project with id of {id}", id);
            return NotFound();
            
        }
        return View(project);
        // Retrieve project form database
        // var project = new Project {ProjectId = id, Name = "Project 1", Description = " First Project" };
        //return View(project);
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
    {
        if (id != project.ProjectId)
        {
            return NotFound(); 
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Projects.Update(project);//update the project
                await _context.SaveChangesAsync();// commit changes

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        return View(project);
    }

    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        //Retrieves the project with the specified id or return null if not found
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound(); 
            
        }
        return View(project);
    }

    [HttpPost("Delete/{ProjectId:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int ProjectId)

    {
        var project = await _context.Projects.FindAsync(ProjectId);
        if (project != null)
        {
            _context.Projects.Remove(project);// remove project from database
            await _context.SaveChangesAsync(); //commit
            return RedirectToAction("Index");
        }

        return NotFound();
    }

    [HttpGet("Search/{searchString}")]
    public async Task<IActionResult> Search(string searchString)
    {
        var projectsQuery =_context.Projects.AsQueryable();
        
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (searchPerformed)
        {
            searchString = searchString.ToLower();
            
            projectsQuery = projectsQuery.Where(p => p.Name.ToLower().Contains(searchString) ||
                                                     p.Description.ToLower().Contains(searchString));
            
        }
        
        // Asynchronus execution means this method does not block the thread while waiting for the database 
        var projects = await projectsQuery.ToListAsync();
        
        //Pass search metedata
        ViewData["SearchString"] = searchString;
        ViewData["SearchPerformed"] = searchPerformed;
        
        
        return View("Index", projects);
    }
    
}