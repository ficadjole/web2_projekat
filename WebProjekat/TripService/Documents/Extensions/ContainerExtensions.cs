using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TripService.Documents.Extensions
{
    public static class ContainerExtensions
    {
        public static void LabelValue(this IContainer container, string label, string? value)
        {
            container.Row(row =>
            {
                row.AutoItem()
                    .Text($"{label}:")
                    .Bold()
                    .FontColor(Colors.Grey.Darken2);

                row.ConstantItem(8);

                row.RelativeItem()
                    .Text(value ?? "-");
            });
        }
    }
}
