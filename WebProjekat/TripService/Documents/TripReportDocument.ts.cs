using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TripService.Documents.Extensions;
using TripService.Documents.Model;
using TripService.Enums;

namespace TripService.Documents
{
    public class TripReportDocument : IDocument
    {
        private readonly TripReportData _data;

        public TripReportDocument(TripReportData data)
        {
            _data = data;
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.PaddingBottom(20).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Trip Report")
                        .FontSize(24).Bold().FontColor(Colors.Blue.Darken2);

                    col.Item().Text($"Generated on {DateTime.UtcNow:dd MMM yyyy}")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                });

                row.ConstantItem(60).Height(60).Svg(GenerateTripIcon());
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(col =>
            {
                // Trip Info
                col.Item().Element(ComposeTripInfo);
                col.Item().PaddingTop(20).Element(ComposeDestinations);
                col.Item().PaddingTop(20).Element(ComposeExpenses);
                col.Item().PaddingTop(20).Element(ComposeChecklist);
            });
        }

        private void ComposeTripInfo(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Background(Colors.Blue.Lighten4)
                    .Padding(10).Text("✈️  Trip Overview")
                    .FontSize(14).Bold().FontColor(Colors.Blue.Darken3);

                col.Item().PaddingTop(10).Border(1).BorderColor(Colors.Grey.Lighten2)
                    .Padding(15).Column(info =>
                    {
                        info.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().LabelValue("Name", _data.Trip.Name);
                                left.Item().PaddingTop(6).LabelValue("Start Date",
                                    _data.Trip.StartDate.ToString("dd MMM yyyy"));
                                left.Item().PaddingTop(6).LabelValue("End Date",
                                    _data.Trip.EndDate.ToString("dd MMM yyyy"));
                            });

                            row.RelativeItem().Column(right =>
                            {
                                right.Item().LabelValue("Planned Budget",
                                    $"${_data.Trip.PlannedBudget:N2}");
                                right.Item().PaddingTop(6).LabelValue("Total Expenses",
                                    $"${_data.TotalExpenses:N2}");
                                right.Item().PaddingTop(6).LabelValue("Remaining Budget",
                                    $"${(_data.Trip.PlannedBudget - _data.TotalExpenses):N2}");
                            });
                        });

                        if (!string.IsNullOrWhiteSpace(_data.Trip.Description))
                        {
                            info.Item().PaddingTop(10).Column(desc =>
                            {
                                desc.Item().Text("Description").Bold().FontColor(Colors.Grey.Darken2);
                                desc.Item().PaddingTop(4).Text(_data.Trip.Description)
                                    .FontColor(Colors.Grey.Medium);
                            });
                        }

                    });
            });
        }

        private void ComposeDestinations(IContainer container)
        {
            if (!_data.Destinations.Any()) return;

            container.Column(col =>
            {
                col.Item().Background(Colors.Teal.Lighten4)
                    .Padding(10).Text("📍  Destinations & Activities")
                    .FontSize(14).Bold().FontColor(Colors.Teal.Darken3);

                foreach (var destination in _data.Destinations)
                {
                    col.Item().PaddingTop(10).Column(dest =>
                    {
                        // Destination header
                        dest.Item().Background(Colors.Grey.Lighten4)
                            .Padding(8).Row(row =>
                            {
                                row.RelativeItem().Text(destination.Name)
                                    .FontSize(12).Bold();
                                row.AutoItem().Text(destination.Location)
                                    .FontColor(Colors.Grey.Medium);
                            });

                        dest.Item().Border(1).BorderColor(Colors.Grey.Lighten2)
                            .Padding(10).Column(destInfo =>
                            {
                                destInfo.Item().Row(row =>
                                {
                                    row.RelativeItem().LabelValue("Arriving",
                                        destination.ArrivingDate.ToString("dd MMM yyyy"));
                                    row.RelativeItem().LabelValue("Leaving",
                                        destination.LeavingDate.ToString("dd MMM yyyy"));
                                });

                                if (!string.IsNullOrWhiteSpace(destination.Notes))
                                    destInfo.Item().PaddingTop(6)
                                        .LabelValue("Notes", destination.Notes);

                                // Activities table
                                if (destination.Activities.Any())
                                {
                                    destInfo.Item().PaddingTop(10)
                                        .Text("Activities").Bold().FontSize(10);

                                    destInfo.Item().PaddingTop(6).Table(table =>
                                    {
                                        table.ColumnsDefinition(cols =>
                                        {
                                            cols.RelativeColumn(3);
                                            cols.RelativeColumn(2);
                                            cols.RelativeColumn(2);
                                            cols.RelativeColumn(1);
                                            cols.RelativeColumn(1);
                                        });

                                        table.Header(header =>
                                        {
                                            foreach (var h in new[] { "Name", "Location", "Date", "Cost", "Status" })
                                            {
                                                header.Cell().Background(Colors.Blue.Lighten3)
                                                    .Padding(5).Text(h).Bold().FontSize(9);
                                            }
                                        });

                                        foreach (var activity in destination.Activities)
                                        {
                                            var isAlternate = destination.Activities.IndexOf(activity) % 2 == 1;
                                            var bg = isAlternate ? Colors.Grey.Lighten5 : Colors.White;

                                            table.Cell().Background(bg).Padding(5)
                                                .Text(activity.Name).FontSize(9);
                                            table.Cell().Background(bg).Padding(5)
                                                .Text(activity.Location).FontSize(9);
                                            table.Cell().Background(bg).Padding(5)
                                                .Text(activity.Date.ToString("dd MMM")).FontSize(9);
                                            table.Cell().Background(bg).Padding(5)
                                                .Text($"${activity.EstimatedCost:N0}").FontSize(9);
                                            table.Cell().Background(bg).Padding(5)
                                                .Text(activity.Status.ToString()).FontSize(9)
                                                .FontColor(GetStatusColor(activity.Status));
                                        }
                                    });
                                }
                                else
                                {
                                    destInfo.Item().PaddingTop(6).Text("No activities planned.")
                                        .FontColor(Colors.Grey.Medium).Italic();
                                }
                            });
                    });
                }
            });
        }

        private void ComposeExpenses(IContainer container)
        {
            if (!_data.Expenses.Any()) return;

            container.Column(col =>
            {
                col.Item().Background(Colors.Orange.Lighten4)
                    .Padding(10).Text("💰  Expenses")
                    .FontSize(14).Bold().FontColor(Colors.Orange.Darken3);

                col.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(3);
                        cols.RelativeColumn(2);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(3);
                    });

                    table.Header(header =>
                    {
                        foreach (var h in new[] { "Name", "Category", "Amount", "Description" })
                        {
                            header.Cell().Background(Colors.Orange.Lighten3)
                                .Padding(5).Text(h).Bold().FontSize(9);
                        }
                    });

                    foreach (var expense in _data.Expenses)
                    {
                        var isAlternate = _data.Expenses.IndexOf(expense) % 2 == 1;
                        var bg = isAlternate ? Colors.Grey.Lighten5 : Colors.White;

                        table.Cell().Background(bg).Padding(5).Text(expense.Name).FontSize(9);
                        table.Cell().Background(bg).Padding(5)
                            .Text(expense.Category.ToString()).FontSize(9);
                        table.Cell().Background(bg).Padding(5)
                            .Text($"${expense.Amount:N2}").FontSize(9).Bold();
                        table.Cell().Background(bg).Padding(5)
                            .Text(expense.Description ?? "-").FontSize(9)
                            .FontColor(Colors.Grey.Medium);
                    }
                });

                col.Item().PaddingTop(8).AlignRight().Row(row =>
                {
                    row.AutoItem().Background(Colors.Orange.Lighten3)
                        .Padding(8).Text($"Total: ${_data.TotalExpenses:N2}")
                        .Bold().FontSize(11);
                });
            });
        }

        private void ComposeChecklist(IContainer container)
        {
            if (_data.ChecklistItems == null || !_data.ChecklistItems.Any()) return;

            container.Column(col =>
            {
                col.Item().Background(Colors.Green.Lighten4)
                    .Padding(10).Text("✅  Packing Checklist")
                    .FontSize(14).Bold().FontColor(Colors.Green.Darken3);

                col.Item().PaddingTop(10).Border(1).BorderColor(Colors.Grey.Lighten2)
                    .Padding(15).Column(list =>
                    {
                        var completed = _data.ChecklistItems.Count(i => i.IsChecked);
                        var total = _data.ChecklistItems.Count;

                        list.Item().PaddingBottom(10).Text($"Completed: {completed}/{total}")
                            .FontColor(Colors.Grey.Medium);

                        var chunks = _data.ChecklistItems
                            .Select((item, index) => new { item, index })
                            .GroupBy(x => x.index / 2)
                            .Select(g => g.Select(x => x.item).ToList())
                            .ToList();

                        foreach (var chunk in chunks)
                        {
                            list.Item().PaddingBottom(4).Row(row =>
                            {
                                foreach (var item in chunk)
                                {
                                    row.RelativeItem().Row(itemRow =>
                                    {
                                        itemRow.AutoItem().PaddingRight(6)
                                            .Text(item.IsChecked ? "☑" : "☐")
                                            .FontColor(item.IsChecked
                                                ? Colors.Green.Darken1
                                                : Colors.Grey.Medium);

                                        itemRow.RelativeItem()
                                            .Text(item.Name)
                                            .FontColor(item.IsChecked
                                                ? Colors.Grey.Medium
                                                : Colors.Black);
                                    });
                                }

                                if (chunk.Count == 1)
                                    row.RelativeItem();
                            });
                        }
                    });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("TripPlanner — ").FontColor(Colors.Grey.Medium);
                text.CurrentPageNumber().FontColor(Colors.Grey.Medium);
                text.Span(" / ").FontColor(Colors.Grey.Medium);
                text.TotalPages().FontColor(Colors.Grey.Medium);
            });
        }

        private string GetStatusColor(ActivityStatus status) => status switch
        {
            ActivityStatus.Finished => Colors.Green.Darken1,
            ActivityStatus.Cancelled => Colors.Red.Medium,
            ActivityStatus.Reserved => Colors.Orange.Darken1,
            _ => Colors.Blue.Darken1
        };

        private string GenerateTripIcon() => @"
        <svg viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'>
            <text y='45' font-size='45'>✈️</text>
        </svg>";
    }
}
