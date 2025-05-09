using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using transferguide.Models;
using transferguide.Services;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace transferguide.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPdfService _pdfService;

    public HomeController(ILogger<HomeController> logger, IPdfService pdfService)
    {
        _logger = logger;
        _pdfService = pdfService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Create a new Transfer model with default values
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

        // Return the Index view with the initialized model
        return View(model);
    }

    [HttpPost]
    public IActionResult Report(Transfer model)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            // If validation fails, return to the Index view with the current model
            return View("Index", model);
        }

        // Ensure lists are initialized to avoid null reference exceptions
        if (model.Year1 == null)
            model.Year1 = new List<Year1Transfer>();

        if (model.Year2 == null)
            model.Year2 = new List<Year2Transfer>();

        if (model.Junior == null)
            model.Junior = new List<JuniorClasses>();

        if (model.Senior == null)
            model.Senior = new List<SeniorClasses>();

        if (model.ImportantNotes == null)
            model.ImportantNotes = new List<string>();
        // Store the model in TempData
        TempData["TransferModel"] = System.Text.Json.JsonSerializer.Serialize(model);
        // Return the Report view with the validated and initialized model
        return View(model);
    }

    [HttpGet]
    public IActionResult GeneratePdf()
    {
        if (TempData["TransferModel"] is string serializedModel)
        {
            var model = System.Text.Json.JsonSerializer.Deserialize<Transfer>(serializedModel);
            if (model != null)
            {
                var pdfBytes = _pdfService.GenerateTransferReportPdf(model);
                return File(pdfBytes, "application/pdf", "TransferReport.pdf");
            }
        }
        
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult DownloadPdf(Transfer model)
    {
        model.Year1 ??= new List<Year1Transfer>();
        model.Year2 ??= new List<Year2Transfer>();
        model.Junior ??= new List<JuniorClasses>();
        model.Senior ??= new List<SeniorClasses>();
        model.ImportantNotes ??= new List<string>();

        var pdfBytes = _pdfService.GenerateTransferReportPdf(model);
        return File(pdfBytes, "application/pdf", "TransferReport.pdf");
    }
}
