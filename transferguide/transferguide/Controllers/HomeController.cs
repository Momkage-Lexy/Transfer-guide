using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using transferguide.Models;
using transferguide.Services;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


using transferguide.Models;

namespace transferguide.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger; 
    private readonly IPdfService _pdfService; 

    public HomeController(
        ILogger<HomeController> logger, IPdfService pdfService)
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

        // Return the Report view with the validated and initialized model
        return View(model);
    }

    [HttpPost]
    public IActionResult DownloadPdf(Transfer model)
    {
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

        // Render the Report view as an HTML string
        var htmlContent = RenderViewAsString("Report", model);

        // Generate the PDF using the PDF service
        var pdfBytes = _pdfService.GenerateTransferReportPdf(model, htmlContent);

        // Return the PDF as a downloadable file
        return File(pdfBytes, "application/pdf", "TransferReport.pdf");
    }

    // Helper method to render a view as a string
    private string RenderViewAsString(string viewName, object model)
    {
        // Set the model for the view
        ViewData.Model = model;

        // Use a StringWriter to capture the rendered view
        using var sw = new StringWriter();

        // Get the view engine from the HTTP context services
        var viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

        // Find the view by name
        var viewResult = viewEngine.FindView(ControllerContext, viewName, false);

        // Throw an exception if the view is not found
        if (viewResult.View == null)
            throw new ArgumentNullException($"{viewName} does not match any available view");

        // Create a ViewContext to render the view
        var viewContext = new ViewContext(
            ControllerContext,
            viewResult.View,
            ViewData,
            TempData,
            sw,
            new HtmlHelperOptions()
        );

        // Render the view asynchronously and wait for completion
        viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

        // Return the rendered view as a string
        return sw.GetStringBuilder().ToString();
    }
}