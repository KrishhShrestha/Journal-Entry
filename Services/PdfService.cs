using Journal_Entry.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;

namespace Journal_Entry.Services
{
    public class PdfService
    {
        // Add this helper method to strip HTML tags
        private string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            // Remove HTML tags
            string stripped = Regex.Replace(html, "<.*?>", string.Empty);

            // Decode HTML entities
            stripped = System.Net.WebUtility.HtmlDecode(stripped);

            // Replace <br>, <br/>, <br />, and </p><p> with newlines
            stripped = Regex.Replace(stripped, @"<br\s*/?>|</p>\s*<p>", "\n", RegexOptions.IgnoreCase);

            return stripped.Trim();
        }

        public byte[] GenerateSingleEntryPdf(JournalEntry entry)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Column(column =>
                    {
                        column.Item().BorderBottom(2).BorderColor(QuestPDF.Helpers.Colors.Blue.Medium)
                            .PaddingBottom(10).Text(entry.Title)
                            .FontSize(24).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);

                        column.Item().PaddingTop(10).Text(text =>
                        {
                            text.Span("Date: ").Bold();
                            text.Span(entry.EntryDate.ToLongDateString());
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Mood: ").Bold();
                            text.Span(entry.PrimaryMood ?? "");
                        });
                    });

                    page.Content().PaddingTop(20).Column(column =>
                    {
                        column.Spacing(10);

                        // Strip HTML tags before splitting into paragraphs
                        var cleanContent = StripHtmlTags(entry.Content ?? "");
                        var paragraphs = cleanContent.Split(new[] { "\r\n", "\n", "\r" },
                            StringSplitOptions.None);

                        foreach (var paragraph in paragraphs)
                        {
                            if (!string.IsNullOrWhiteSpace(paragraph))
                            {
                                column.Item().Text(paragraph).LineHeight(1.5f);
                            }
                            else
                            {
                                column.Item().Height(10);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateAllEntriesPdf(List<JournalEntry> entries)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("My Journal Entries")
                            .FontSize(28).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                        column.Item().PaddingTop(5).LineHorizontal(2).LineColor(QuestPDF.Helpers.Colors.Blue.Medium);
                    });

                    page.Content().Column(column =>
                    {
                        bool firstEntry = true;
                        foreach (var entry in entries.OrderByDescending(e => e.EntryDate))
                        {
                            if (!firstEntry)
                            {
                                column.Item().PageBreak();
                            }
                            firstEntry = false;

                            column.Item().Column(entryColumn =>
                            {
                                entryColumn.Item().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                                    .PaddingBottom(5).Text(entry.Title)
                                    .FontSize(18).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                                entryColumn.Item().PaddingTop(10).Text(text =>
                                {
                                    text.Span("Date: ").Bold();
                                    text.Span(entry.EntryDate.ToLongDateString());
                                });

                                entryColumn.Item().Text(text =>
                                {
                                    text.Span("Mood: ").Bold();
                                    text.Span(entry.PrimaryMood ?? "");
                                });

                                entryColumn.Item().PaddingTop(15).Column(contentColumn =>
                                {
                                    // Strip HTML tags before splitting into paragraphs
                                    var cleanContent = StripHtmlTags(entry.Content ?? "");
                                    var paragraphs = cleanContent.Split(new[] { "\r\n", "\n", "\r" },
                                        StringSplitOptions.None);

                                    foreach (var paragraph in paragraphs)
                                    {
                                        if (!string.IsNullOrWhiteSpace(paragraph))
                                        {
                                            contentColumn.Item().Text(paragraph).LineHeight(1.5f);
                                        }
                                        else
                                        {
                                            contentColumn.Item().Height(10);
                                        }
                                    }
                                });
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}