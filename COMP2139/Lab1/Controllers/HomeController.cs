using System.Diagnostics;
using Lab1.Areas.ProjectManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lab1.Models;

namespace Lab1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        searchType = searchType?.Trim().ToLower();
        
        searchString = searchString?.Trim().ToLower();

        if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchString))
        {
            return RedirectToAction(nameof(Index));
            
        }
        
        if (searchType == "projects")
        {
            return RedirectToAction(nameof(ProjectController.Search), "Project", new { searchString});
        }
        

        else if (searchType == "tasks")
        {
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask", new { searchString });
        }
        
        return RedirectToAction(nameof(Index),"Home");
        
    }
}