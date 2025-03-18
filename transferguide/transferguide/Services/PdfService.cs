using DinkToPdf; 
using DinkToPdf.Contracts; 
using transferguide.Models;

namespace transferguide.Services
{
    // Define an interface for the PDF service
    public interface IPdfService
    {
        // Declare a method to generate a PDF report for a transfer
        public byte[] GenerateTransferReportPdf(Transfer model, string htmlContent);
    }

    // Implement the PDF service
    public class PdfService : IPdfService
    {
        // Declare a private field for the PDF converter
        private readonly IConverter _converter;

        // Constructor to inject the PDF converter dependency
        public PdfService(IConverter converter)
        {
            _converter = converter; // Initialize the converter
        }

        // Implement the method to generate a PDF report
        public byte[] GenerateTransferReportPdf(Transfer model, string htmlContent)
        {
            // Create a new HTML-to-PDF document configuration
            var doc = new HtmlToPdfDocument()
            {
                // Global settings for the PDF document
                GlobalSettings = {
                    PaperSize = PaperKind.Letter, // Set the paper size to Letter
                    Margins = new MarginSettings() { Top = 10, Bottom = 10, Left = 10, Right = 10 }, // Set margins
                    Orientation = Orientation.Portrait, // Set the orientation to Portrait
                },
                // Object settings for the content of the PDF
                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlContent, // Set the HTML content to be converted to PDF
                        WebSettings = { DefaultEncoding = "utf-8" }, // Set the default encoding to UTF-8
                        PagesCount = true, // Enable page numbering
                        FooterSettings = { FontSize = 10, Right = "Page [page] of [toPage]" } // Configure footer with page numbers
                    }
                }
            };

            // Convert the HTML document to a PDF byte array
            byte[] pdf = _converter.Convert(doc);

            // Return the generated PDF as a byte array
            return pdf;
        }
    }
}