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
        private string GetFullCollegeName(string collegeCode)
        {
            return collegeCode switch
            {
                "CCC" => "Chemeketa Community College",
                "LBCC" => "Linn-Benton Community College",
                "PCC" => "Portland Community College",
                _ => collegeCode
            };
        }

        public byte[] GenerateTransferReportPdf(Transfer model)
        {
            string fullCollegeName = GetFullCollegeName(model.CollegeName);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Montserrat"));

                    page.Content().Column(col =>
                    {
                        col.Item().Background("#DB0A29").Padding(10).AlignCenter().Text("TRANSFER GUIDE")
                            .FontSize(26).Bold().FontColor("#FFFFFF");

                        col.Item().Background("#DB0A29").AlignCenter().Text($"{fullCollegeName} → Western Oregon University")
                            .FontSize(18).Bold().FontColor("#FFFFFF");

                        col.Item().Text($"Earn a {model.Degree} in");

                        col.Item().Text($"{model.Major}")
                            .FontSize(20).Bold().FontColor("#333333");

                        col.Item().Row(row =>
                        {
                            row.RelativeItem(5).Column(leftCol =>
                            {
                                leftCol.Item().Text("STEP 1")
                                    .FontSize(16).Bold().FontColor("#DB0A29");

                                leftCol.Item().Text($"⬥ Complete an AS or AAOT/ASOT at {fullCollegeName} (see Sample Plan of courses on next page).");
                                
                                leftCol.Item().Background("#C4C6C8").Padding(10).Column(help =>
                                {
                                    help.Item().Text("NEED HELP").Bold().FontSize(14).FontColor("#DB0A29");
                                    help.Item().Text($"I have questions about my major classes.").Bold();
                                    help.Item().Text($"Contact our faculty advisor {model.AdvisorName} at: {model.AdvisorEmail}");
                                    help.Item().Text($"I have general questions about WOU or applying for admission").Bold();
                                    help.Item().Text($"Contact us at admissons@wou.edu or schedule an appointment with our transfer specialist, Briana, at https://calendly.com/navareteb/admissions");
                                    help.Item().Text($"I have questions about how my credits will transfer and fulfill General Education requirement.").Bold();
                                    help.Item().Text($"Contact our General Education Program at gened@wou.edu or visit wou.edu/gened/faq");
                                });
                                
                                /*Logo goes here*/
                                leftCol.Item().Text("advising@wou.edu");
                            });

                            row.RelativeItem(7).Column(rightCol =>
                            {
                                rightCol.Item().Text("STEP 2")
                                    .FontSize(16).Bold().FontColor("#DB0A29");
                                
                                rightCol.Item().Text($"⬥ Apply to transfer in your final year at {fullCollegeName}").Bold();
                                rightCol.Item().Text("⬥ To apply, you will need:").Bold();
                                rightCol.Item().Text("• WOU Admission Application");
                                rightCol.Item().Text($"• Transcripts from {fullCollegeName}");
                                rightCol.Item().Text("• Add WOU to your FAFSA. Our school code is 003209");

                                rightCol.Item().Text("IMPORTANT NOTES").Bold();
                                foreach(var note in model.ImportantNotes)
                                {
                                    rightCol.Item().Text($"• {note}");
                                }
                                rightCol.Item().Text("RESOURCES and INFORMATION").Bold();
                                rightCol.Item().Text($"• AAOT, ASOT, and AS degrees from CCC will Complete all General Education requirements at WOU");
                                rightCol.Item().Text($"• You do not need to complete a degree to transfer to WOU. Look up how your classes transfer here: https://wou.edu/admission/transfer/how-credits-transfer/");
                                rightCol.Item().Text($"• Want to take classes at both CCC and WOU? Checkout our Degree Partnership Program: https://wou.edu/admission/degree-partnership-program/");
                            });
                        });

                        col.Item().PageBreak();

                        col.Item().AlignCenter().Text("SAMPLE 4-YEAR PLAN")
                            .FontSize(20).Bold().FontColor("#808080");

                        col.Item().Text("YEAR ONE").Bold().FontSize(16).FontColor("#DB0A29");
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Course").Bold();
                                header.Cell().Text("Credits").Bold();
                                header.Cell().Text("WOU Equivalent").Bold();
                            });

                            if (model.Year1 != null)
                            {
                                foreach (var course in model.Year1)
                                {
                                    table.Cell().Text(course.TransferCourse);
                                    table.Cell().Text(course.TransferCredits.ToString());
                                    table.Cell().Text(course.WouEquivalent);
                                }
                            }
                        });

                        col.Item().Text("YEAR TWO").Bold().FontSize(16).FontColor("#DB0A29");
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                            });
                            
                            table.Header(header =>
                            {
                                header.Cell().Text("Course").Bold();
                                header.Cell().Text("Credits").Bold();
                                header.Cell().Text("WOU Equivalent").Bold();
                            });

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
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
