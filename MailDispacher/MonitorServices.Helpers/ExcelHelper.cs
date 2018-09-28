using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Helpers
{
    public class ExcelHelper
    {
        public byte[] CreaExcelMancanti(MagazzinoDS ds)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                        new Column // Id column
                        {
                            Min = 1,
                            Max = 1,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Id column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column // Id column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column // Id column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 40,
                            CustomWidth = false
                        },
                        new Column // Id column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 60,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 6,
                            Max = 6,
                            Width = 15,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Mancanti" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("IDMAGAZZ", CellValues.String, 2),
                    ConstructCell("MOdello", CellValues.String, 2),
                    ConstructCell("Descrizione", CellValues.String, 2),
                    ConstructCell("Magazzino", CellValues.String, 2),
                    ConstructCell("DescMagaz", CellValues.String, 2),
                    ConstructCell("Quantità", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (MagazzinoDS.MAGAZZINONEGATIVORow elemento in ds.MAGAZZINONEGATIVO)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.IDMAGAZZ.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.MODELLO, CellValues.String, 1),
                        ConstructCell(elemento.DESMAGAZZ, CellValues.String, 1),
                        ConstructCell(elemento.CODICEMAG, CellValues.String, 1),
                        ConstructCell(elemento.DESMAGAZZ, CellValues.String, 1),
                        ConstructCell(elemento.QESI.ToString(), CellValues.String, 1));

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }

        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
    }
}
