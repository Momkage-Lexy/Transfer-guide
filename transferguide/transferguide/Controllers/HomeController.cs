using Microsoft.AspNetCore.Mvc;
using DinkToPdf;
using DinkToPdf.Contracts;
using transferguide.Models;
using System.IO;

namespace transferguide.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new Transfer
        {
            CollegeName = string.Empty,
            Degree = string.Empty,
            Major = string.Empty,
            AdvisorName = string.Empty,
            AdvisorEmail = string.Empty,
            ImportantNotes = new List<string>(),
            Year1 = new List<Year1Transfer>
            {
                new Year1Transfer
                {
                    TransferCourse = string.Empty,
                    TransferCredits = 0,
                    WouEquivalent = string.Empty
                }
            },
             Year2 = new List<Year2Transfer>
            {
                new Year2Transfer
                {
                    TransferCourse = string.Empty,
                    TransferCredits = 0,
                    WouEquivalent = string.Empty
                }
            },
            Junior = new List<JuniorClasses>
            {
                new JuniorClasses
                {
                    Course = string.Empty,
                    Credits = 0
                }
            },
            Senior = new List<SeniorClasses>
            {
                new SeniorClasses
                {
                    Course = string.Empty,
                    Credits = 0
                }
            }
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Report(Transfer model)
    {
        if (!ModelState.IsValid)
        {
            // If validation fails, return to the form
            return View("Index", model);
        }

        if (model.Year1 == null)
        {
            model.Year1 = new List<Year1Transfer>();
        }
        if (model.Year2 == null)
        {
            model.Year2 = new List<Year2Transfer>();
        }
        if (model.Junior == null)
        {
            model.Junior = new List<JuniorClasses>();
        }
        if (model.Senior == null)
        {
            model.Senior = new List<SeniorClasses>();
        }
        if (model.ImportantNotes == null)
        {
            model.ImportantNotes = new List<string>();
        }
        
        return View(model);
    }
}
