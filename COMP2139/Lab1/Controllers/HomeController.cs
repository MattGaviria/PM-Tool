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
        _logger.LogInformation("Accessed HomeController Index at {Time}", DateTime.Now);
        return View();
    }
    
  
    public IActionResult About()
    {
        _logger.LogInformation("Accessed HomeController About at {Time}", DateTime.Now);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("Accessed HomeController Error at {Time}", DateTime.Now);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        _logger.LogInformation("Accessed HomeController GeneralSearch at {Time}", DateTime.Now);
        searchType = searchType?.Trim().ToLower();
        
        searchString = searchString?.Trim().ToLower();

        if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchString))
        {
            return RedirectToAction(nameof(Index));
            
        }
        
        if (searchType == "projects")
        {
            return RedirectToAction(nameof(ProjectController.Search), "Project", new { area="ProjectManagement", searchString});
        }
        

        else if (searchType == "tasks")
        {
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask", new { area="ProjectManagement", searchString });
        }
        
        return RedirectToAction(nameof(Index),"Home");
        
    }

    [HttpGet]
    public IActionResult NotFound(int statusCode)
    {
        _logger.LogWarning("NotFound invoked at {Time}", DateTime.Now);
        if (statusCode == 404)
        {
            return View("NotFound");
        }
        return View(Error);
    }
}