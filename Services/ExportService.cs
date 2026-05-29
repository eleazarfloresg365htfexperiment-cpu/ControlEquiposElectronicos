using ClosedXML.Excel;
using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPdfColors = QuestPDF.Helpers.Colors;

namespace ControlEquiposElectronicos.Services;

public class ExportService : IExportService
{
    // ─────────────────────────────────────────────────────────────
    //  EXCEL
    // ─────────────────────────────────────────────────────────────
    public Task<string> ExportarReportesExcelAsync(List<ReporteFallaListadoDto> reportes)
    {
        return Task.Run(() =>
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Reportes de Falla");

            var titulo = ws.Range("A1:G1").Merge();
            titulo.Value = "REPORTE DE FALLAS TÉCNICAS";
            titulo.Style.Font.Bold = true;
            titulo.Style.Font.FontSize = 14;
            titulo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titulo.Style.Fill.BackgroundColor = XLColor.FromHtml("#6C3FC5");
            titulo.Style.Font.FontColor = XLColor.White;
            ws.Row(1).Height = 28;

            var subtitulo = ws.Range("A2:G2").Merge();
            subtitulo.Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
            subtitulo.Style.Font.Italic = true;
            subtitulo.Style.Font.FontSize = 9;
            subtitulo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            subtitulo.Style.Fill.BackgroundColor = XLColor.FromHtml("#EDE9F8");

            string[] headers = { "#", "ID", "Código Equipo", "Título", "Estado", "Prioridad", "Fecha Reporte" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(3, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#3B1F8C");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
            ws.Row(3).Height = 18;

            for (int i = 0; i < reportes.Count; i++)
            {
                var r = reportes[i];
                int row = i + 4;
                var bgColor = i % 2 == 0 ? XLColor.White : XLColor.FromHtml("#F4F1FB");

                ws.Cell(row, 1).Value = i + 1;
                ws.Cell(row, 2).Value = r.Id;
                ws.Cell(row, 3).Value = r.CodigoEquipo;
                ws.Cell(row, 4).Value = r.Titulo;
                ws.Cell(row, 5).Value = r.EstadoReporte;
                ws.Cell(row, 6).Value = r.Prioridad;
                ws.Cell(row, 7).Value = r.FechaReporte.ToString("dd/MM/yyyy");

                ws.Cell(row, 6).Style.Font.FontColor = r.Prioridad.ToLower() switch
                {
                    "alta" => XLColor.DarkRed,
                    "media" => XLColor.DarkOrange,
                    "baja" => XLColor.DarkGreen,
                    _ => XLColor.Black
                };
                ws.Cell(row, 6).Style.Font.Bold = true;

                ws.Cell(row, 5).Style.Font.FontColor = r.EstadoReporte.ToLower() switch
                {
                    "pendiente" => XLColor.DarkOrange,
                    "resuelto" => XLColor.DarkGreen,
                    "en proceso" => XLColor.DarkBlue,
                    _ => XLColor.Black
                };

                for (int c = 1; c <= 7; c++)
                {
                    var cell = ws.Cell(row, c);
                    cell.Style.Fill.BackgroundColor = bgColor;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#CCCCCC");
                    cell.Style.Alignment.Horizontal = c == 4
                        ? XLAlignmentHorizontalValues.Left
                        : XLAlignmentHorizontalValues.Center;
                }
            }

            int totalRow = reportes.Count + 4;
            var totalRange = ws.Range(totalRow, 1, totalRow, 6).Merge();
            totalRange.Value = $"Total de registros: {reportes.Count}";
            totalRange.Style.Font.Bold = true;
            totalRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#EDE9F8");
            totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 8;
            ws.Column(3).Width = 18;
            ws.Column(4).Width = 38;
            ws.Column(5).Width = 16;
            ws.Column(6).Width = 12;
            ws.Column(7).Width = 18;

            string carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ControlEquipos");
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta,
                $"ReportesFalla_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            workbook.SaveAs(ruta);
            return ruta;
        });
    }

    // ─────────────────────────────────────────────────────────────
    //  PDF
    // ─────────────────────────────────────────────────────────────
    public Task<string> ExportarReportesPdfAsync(List<ReporteFallaListadoDto> reportes)
    {
        return Task.Run(() =>
        {
            QuestPDF.Settings.License = LicenseType.Community;

            string carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ControlEquipos");
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta,
                $"ReportesFalla_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1.5f, Unit.Centimetre);
                    page.DefaultTextStyle(t => t.FontFamily("Arial").FontSize(9));

                    // ── ENCABEZADO ────────────────────────────────
                    page.Header().Column(col =>
                    {
                        col.Item().Background(QuestPdfColors.DeepPurple.Darken3).Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(inner =>
                            {
                                inner.Item()
                                     .Text("ACADEMIA - CONTROL DE EQUIPOS ELECTRÓNICOS")
                                     .FontColor(QuestPdfColors.White)
                                     .FontSize(15)
                                     .Bold();
                                inner.Item()
                                     .Text("Reporte de Fallas Técnicas")
                                     .FontColor(QuestPdfColors.Purple.Lighten3)
                                     .FontSize(10);
                            });
                            row.ConstantItem(160).AlignRight().Column(inner =>
                            {
                                inner.Item()
                                     .Text($"Fecha: {DateTime.Now:dd/MM/yyyy}")
                                     .FontColor(QuestPdfColors.White)
                                     .FontSize(9);
                                inner.Item()
                                     .Text($"Hora:  {DateTime.Now:HH:mm:ss}")
                                     .FontColor(QuestPdfColors.White)
                                     .FontSize(9);
                                inner.Item()
                                     .Text($"Total registros: {reportes.Count}")
                                     .FontColor(QuestPdfColors.Yellow.Lighten2)
                                     .FontSize(9)
                                     .Bold();
                            });
                        });
                        col.Item().Height(8);
                    });

                    // ── CONTENIDO ─────────────────────────────────
                    page.Content().Column(col =>
                    {
                        var porEstado = reportes
                            .GroupBy(r => r.EstadoReporte)
                            .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                            .ToList();

                        col.Item().Background(QuestPdfColors.DeepPurple.Lighten4).Padding(8).Row(resumen =>
                        {
                            resumen.RelativeItem().Text("Resumen por estado:").Bold().FontSize(9);
                            foreach (var e in porEstado)
                                resumen.ConstantItem(120).Text($"{e.Estado}: {e.Cantidad}").FontSize(9);
                        });

                        col.Item().Height(8);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(25);
                                cols.ConstantColumn(40);
                                cols.RelativeColumn(2);
                                cols.RelativeColumn(5);
                                cols.RelativeColumn(2.5f);
                                cols.RelativeColumn(1.5f);
                                cols.RelativeColumn(2);
                            });

                            tabla.Header(header =>
                            {
                                void Th(string texto) =>
                                    header.Cell()
                                          .Background(QuestPdfColors.DeepPurple.Medium)
                                          .Padding(5)
                                          .Text(texto)
                                          .FontColor(QuestPdfColors.White)
                                          .FontSize(8)
                                          .Bold()
                                          .AlignCenter();

                                Th("#");
                                Th("ID");
                                Th("Código Equipo");
                                Th("Título");
                                Th("Estado");
                                Th("Prioridad");
                                Th("Fecha");
                            });

                            for (int i = 0; i < reportes.Count; i++)
                            {
                                var r = reportes[i];
                                var bg = i % 2 == 0
                                    ? QuestPdfColors.Grey.Lighten5
                                    : QuestPdfColors.White;

                                string colorPrioridad = r.Prioridad.ToLower() switch
                                {
                                    "alta" => QuestPdfColors.Red.Darken2,
                                    "media" => QuestPdfColors.Orange.Darken2,
                                    "baja" => QuestPdfColors.Green.Darken2,
                                    _ => QuestPdfColors.Grey.Darken2
                                };

                                string colorEstado = r.EstadoReporte.ToLower() switch
                                {
                                    "pendiente" => QuestPdfColors.Orange.Darken2,
                                    "resuelto" => QuestPdfColors.Green.Darken2,
                                    "en proceso" => QuestPdfColors.Blue.Darken2,
                                    _ => QuestPdfColors.Grey.Darken2
                                };

                                void Td(string texto, string? colorTexto = null, bool bold = false)
                                {
                                    var cell = tabla.Cell()
                                                    .Background(bg)
                                                    .BorderBottom(0.5f)
                                                    .BorderColor(QuestPdfColors.Grey.Lighten2)
                                                    .Padding(4);
                                    var txt = cell.Text(texto).FontSize(8).AlignCenter();
                                    if (colorTexto != null) txt.FontColor(colorTexto);
                                    if (bold) txt.Bold();
                                }

                                Td((i + 1).ToString());
                                Td(r.Id.ToString());
                                Td(r.CodigoEquipo);

                                tabla.Cell()
                                     .Background(bg)
                                     .BorderBottom(0.5f)
                                     .BorderColor(QuestPdfColors.Grey.Lighten2)
                                     .Padding(4)
                                     .Text(r.Titulo)
                                     .FontSize(8);

                                Td(r.EstadoReporte, colorEstado, true);
                                Td(r.Prioridad, colorPrioridad, true);
                                Td(r.FechaReporte.ToString("dd/MM/yyyy"));
                            }
                        });
                    });

                    // ── PIE DE PÁGINA ─────────────────────────────
                    page.Footer().Background(QuestPdfColors.DeepPurple.Darken3).Padding(6).Row(row =>
                    {
                        row.RelativeItem()
                           .Text("Control de Equipos Electrónicos — Documento generado automáticamente")
                           .FontColor(QuestPdfColors.White)
                           .FontSize(7);
                        row.ConstantItem(60).AlignRight().Text(t =>
                        {
                            t.Span("Pág. ").FontColor(QuestPdfColors.White).FontSize(7);
                            t.CurrentPageNumber().FontColor(QuestPdfColors.White).FontSize(7);
                            t.Span(" / ").FontColor(QuestPdfColors.White).FontSize(7);
                            t.TotalPages().FontColor(QuestPdfColors.White).FontSize(7);
                        });
                    });
                });
            }).GeneratePdf(ruta);

            return ruta;
        });
    }
}