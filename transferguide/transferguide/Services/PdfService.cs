using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using transferguide.Models;

namespace transferguide.Services
{
    public interface IPdfService
    {
        byte[] GenerateTransferReportPdf(Transfer model);
    }

    public class PdfService : IPdfService
    {
        // Helper method to get full college name from abbreviation
        private string GetFullCollegeName(string collegeCode)
        {
            return collegeCode switch
            {
                "CCC" => "Chemeketa Community College",
                "LBCC" => "Linn-Benton Community College",
                "PCC" => "Portland Community College",
                _ => collegeCode // Fallback to the code if not recognized
            };
        }

        public byte[] GenerateTransferReportPdf(Transfer model)
        {
            // Get full college name
            string fullCollegeName = GetFullCollegeName(model.CollegeName);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().Text("TRANSFER GUIDE")
                            .FontSize(22).Bold().FontColor(Colors.Red.Medium);

                        col.Item().AlignCenter().PaddingTop(5).Text($"{fullCollegeName} → Western Oregon University")
                            .FontSize(16).Bold();

                        col.Item().AlignCenter().PaddingTop(5).Text($"Earn a {model.Degree} in {model.Major}")
                            .FontSize(18).Bold();

                        col.Item().PaddingTop(15).Text("Step 1").Bold().FontSize(16).FontColor(Colors.Red.Medium);
                        col.Item().PaddingVertical(5).Text($"Complete an AS or AAOT/ASOT at {fullCollegeName} (see sample plan on next page).");

                        col.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(help =>
                        {
                            help.Item().Text("NEED HELP").Bold().FontSize(14).FontColor(Colors.Red.Medium);
                            help.Item().PaddingTop(5).Text($"• Questions about major classes: {model.AdvisorName} ({model.AdvisorEmail})");
                            help.Item().PaddingTop(2).Text("• General Admission: admissions@wou.edu");
                            help.Item().PaddingTop(2).Text("• General Education: gened@wou.edu");
                        });

                        col.Item().PaddingTop(15).Text("Step 2").Bold().FontSize(16).FontColor(Colors.Red.Medium);
                        col.Item().PaddingVertical(5).Text($"Apply to transfer final year at {fullCollegeName}.");

                        col.Item().PaddingTop(15).Text("Important Notes").Bold().FontSize(14);
                        if(model.ImportantNotes != null && model.ImportantNotes.Any())
                        {
                            col.Item().Column(c =>
                            {
                                foreach(var note in model.ImportantNotes)
                                {
                                    c.Item().PaddingVertical(2).Text("• " + note);
                                }
                            });
                        }

                        col.Item().PaddingTop(15).Text("Resources and Information").Bold().FontSize(14);
                        col.Item().Column(c =>
                        {
                            c.Item().Text($"• AAOT, ASOT, and AS degrees from {fullCollegeName} complete all General Education requirements at WOU.");
                            c.Item().Text("• Transfer details: https://wou.edu/admission/transfer/how-credits-transfer/");
                            c.Item().Text("• Degree Partnership Program: https://wou.edu/admission/degree-partnership-program/");
                        });

                        // Page Break and Page 2
                        col.Item().PageBreak();

                        col.Item().AlignCenter().Text("SAMPLE 4-YEAR PLAN")
                            .FontSize(20).Bold().FontColor(Colors.Red.Medium);

                        col.Item().PaddingTop(10).Text("Years One and Two at " + fullCollegeName)
                            .FontSize(16).Bold().FontColor(Colors.Red.Medium);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn();
                                cols.ConstantColumn(50);
                                cols.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text($"{fullCollegeName} Course").Bold();
                                header.Cell().Text("Credits").Bold();
                                header.Cell().Text("WOU Equivalent").Bold();
                            });

                            // Year 1 courses
                            if (model.Year1 != null)
                            {
                                foreach (var course in model.Year1)
                                {
                                    table.Cell().Text(course.TransferCourse);
                                    table.Cell().Text(course.TransferCredits.ToString());
                                    table.Cell().Text(course.WouEquivalent);
                                }
                            }

                            // Year 2 courses
                            if (model.Year2 != null)
                            {
                                foreach (var course in model.Year2)
                                {
                                    table.Cell().Text(course.TransferCourse);
                                    table.Cell().Text(course.TransferCredits.ToString());
                                    table.Cell().Text(course.WouEquivalent);
                                }
                            }
                        });

                        col.Item().PaddingTop(15).Text("Junior & Senior Years at WOU").Bold().FontSize(14).FontColor(Colors.Red.Medium);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn();
                                cols.ConstantColumn(50);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("WOU Course").Bold();
                                header.Cell().Text("Credits").Bold();
                            });

                            if (model.Junior != null)
                            {
                                foreach (var course in model.Junior)
                                {
                                    table.Cell().Text(course.Course);
                                    table.Cell().Text(course.Credits.ToString());
                                }
                            }
                            
                            if (model.Senior != null)
                            {
                                foreach (var course in model.Senior)
                                {
                                    table.Cell().Text(course.Course);
                                    table.Cell().Text(course.Credits.ToString());
                                }
                            }
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}