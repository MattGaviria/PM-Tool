using Lab1.Areas.ProjectManagement.Models;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("Project")]
public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Index action will retrieve a listing of projects (database)
    /// </summary>
    
    [HttpGet("")]
    public IActionResult Index()
    {
        // Retrieve all projects from the database
        var projects=_context.Projects.ToList();
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
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {
        //Persist new project to the database
        if (ModelState.IsValid)
        {
            _context.Projects.Add(project); // add new project to database
            _context.SaveChanges(); //persists the changes to database
            return RedirectToAction("Index");
        }
        
        return View("Index");
    }
    
    //CRUD CREATE - READ -UPDATE - DELETE
    
    [HttpGet("Details/{id:int}")]
    public IActionResult Details(int id)
    {
        // retrieves the project with the specified id or returns null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
            
        }
        return View(project);
        // Retrieve project form database
        // var project = new Project {ProjectId = id, Name = "Project 1", Description = " First Project" };
        //return View(project);
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectID, name, Description")] Project project)
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
                _context.SaveChanges();// commit changes

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
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

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }

    [HttpGet("Delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        //Retrieves the project with the specified id or return null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound(); 
            
        }
        return View(project);
    }

    [HttpPost("Delete/{ProjectId:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int ProjectId)

    {
        var project = _context.Projects.Find(ProjectId);
        if (project != null)
        {
            _context.Projects.Remove(project);// remove project from database
            _context.SaveChanges(); //commit
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