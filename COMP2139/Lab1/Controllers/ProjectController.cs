using Lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

public class ProjectController : Controller
{
    /// <summary>
    /// Index action will retrive a listing of projects (datanase)
    /// </summary>
    
    [HttpGet]
    public IActionResult Index()
    {
        var projects = new List<Project>
        {
            new Project { ProjectId = 1, Name = "Project1", Description = " First Project 1" },
        };
        return View(projects);
    }
    
}