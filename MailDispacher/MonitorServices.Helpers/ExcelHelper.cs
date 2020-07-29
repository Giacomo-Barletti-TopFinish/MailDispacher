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
        public byte[] CreaExcelMagazziniGiacenzeBrandManager(MagazzinoDS ds)
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
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column // Id column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Id column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column // Id column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 6,
                            Max = 6,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
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

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Giacenze Brand Manger" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("IDMAGAZZ", CellValues.String, 2),
                    ConstructCell("MODELLO", CellValues.String, 2),
                    ConstructCell("DESCRIZIONE", CellValues.String, 2),
                    ConstructCell("MAGAZZINO", CellValues.String, 2),
                    ConstructCell("DESCRIZIONE MAGAZZINO", CellValues.String, 2),
                    ConstructCell("ESISTENA", CellValues.String, 2),
                    ConstructCell("DISPONIBILE SU ESISTENZA", CellValues.String, 2),
                    ConstructCell("COSTO", CellValues.String, 2),
                    ConstructCell("VALORE", CellValues.String, 2),
                    ConstructCell("VALORE DISPONIBILE", CellValues.String, 2),
                    ConstructCell("DATA CARICO", CellValues.String, 2),
                    ConstructCell("DATA SCARICO", CellValues.String, 2),
                    ConstructCell("SEGNALATORE", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (MagazzinoDS.GIACENZA_BRAND_MANAGERRow elemento in ds.GIACENZA_BRAND_MANAGER)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.IDMAGAZZ, CellValues.String, 1),
                        ConstructCell(elemento.MODELLO, CellValues.String, 1),
                        ConstructCell(elemento.DESMAGAZZ, CellValues.String, 1),
                        ConstructCell(elemento.CODICEMAG, CellValues.String, 1),
                        ConstructCell(elemento.DESTABMAG, CellValues.String, 1),
                        ConstructCell(elemento.QESI.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.QTOT_DISP_ESI.ToString(), CellValues.String, 1),

                   ConstructCell(elemento.IsCOSTONull() ? string.Empty : elemento.COSTO.ToString(), CellValues.String, 1),
                   ConstructCell(elemento.IsVALORENull() ? string.Empty : elemento.VALORE.ToString(), CellValues.String, 1),
                   ConstructCell(elemento.IsVALORE_DISPNull() ? string.Empty : elemento.VALORE_DISP.ToString(), CellValues.String, 1),


                        ConstructCell(elemento.IsDATA_CARICONull() ? string.Empty : elemento.DATA_CARICO.ToShortDateString(), CellValues.String, 1),
                    ConstructCell(elemento.IsDATA_SCARICONull() ? String.Empty : elemento.DATA_SCARICO.ToShortDateString(), CellValues.String, 1),
                    ConstructCell(elemento.IsSEGNALATORENull() ? string.Empty : elemento.SEGNALATORE, CellValues.String, 1));
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

        public byte[] CreaExcelScartiDifettosi(MagazzinoDS ds)
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

                Columns columns = new Columns();
                for (int i = 0; i < ds.SCARTIDIFETTOSI.Columns.Count; i++)
                {
                    Column c = new Column();
                    UInt32Value u = new UInt32Value((uint)(i + 1));
                    c.Min = u;
                    c.Max = u;
                    c.Width = 15;

                    columns.Append(c);
                }


                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Scarti difettosi" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("MODELLO", CellValues.String, 2),
                    ConstructCell("DESCRIZIONE", CellValues.String, 2),
                    ConstructCell("COMMESSA", CellValues.String, 2),
                    ConstructCell("RIFERIMENTO", CellValues.String, 2),
                    ConstructCell("AZIENDA", CellValues.String, 2),
                    ConstructCell("DATA", CellValues.String, 2),
                    ConstructCell("QUANTITA'", CellValues.String, 2),
                    ConstructCell("ODL", CellValues.String, 2),
                    ConstructCell("REPARTO", CellValues.String, 2),
                    ConstructCell("QUANTITA' ODL", CellValues.String, 2),
                    ConstructCell("QUANTITA TERMINATA ODL", CellValues.String, 2),
                    ConstructCell("TOTALE DIFETTOSA", CellValues.String, 2),
                    ConstructCell("TOTALE MANCANTI", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (MagazzinoDS.SCARTIDIFETTOSIRow elemento in ds.SCARTIDIFETTOSI)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.MODELLO, CellValues.String, 1),
                        ConstructCell(elemento.DESMAGAZZ, CellValues.String, 1),
                        ConstructCell(elemento.NOMECOMMESSA, CellValues.String, 1),
                        ConstructCell(elemento.IsRIFERIMENTONull()?string.Empty:elemento.RIFERIMENTO, CellValues.String, 1),
                        ConstructCell(elemento.AZIENDA, CellValues.String, 1),
                        ConstructCell(elemento.DATAFLUSSOMOVFASE.ToShortDateString(), CellValues.String, 1),
                        ConstructCell(elemento.QUANTITA.ToString(), CellValues.Number, 1),
                        ConstructCell(elemento.NUMMOVFASE, CellValues.String, 1),
                        ConstructCell(elemento.REPARTO, CellValues.String, 1),
                        ConstructCell(elemento.QTANTITAODL.ToString(), CellValues.Number, 1),
                        ConstructCell(elemento.QUANTITATERMINATA.ToString(), CellValues.Number, 1),
                        ConstructCell(elemento.TOTALEDIFETTOSA.ToString(), CellValues.Number, 1),
                        ConstructCell(elemento.TOTALEMANCANTI.ToString(), CellValues.Number, 1));
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

        public byte[] CreaExcelMagazziniGiacenze(MagazzinoDS ds)
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
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column // Id column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Id column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column // Id column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
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

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Giacenze" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Azienda", CellValues.String, 2),
                    ConstructCell("IDMAGAZZ", CellValues.String, 2),
                    ConstructCell("Modello", CellValues.String, 2),
                    ConstructCell("Descrizione", CellValues.String, 2),
                    ConstructCell("Esistenza al netto impieghi", CellValues.String, 2),
                    ConstructCell("Soglia giacenza", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (MagazzinoDS.MAGAZZINOGIACENZARow elemento in ds.MAGAZZINOGIACENZA)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.AZIENDA, CellValues.String, 1),
                        ConstructCell(elemento.IDMAGAZZ.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.MODELLO, CellValues.String, 1),
                        ConstructCell(elemento.DESMAGAZZ, CellValues.String, 1),
                    ConstructCell(elemento.QTOT_DISP_ESI.ToString(), CellValues.String, 1),
                    ConstructCell(elemento.GIACENZA.ToString(), CellValues.String, 1));

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
        public byte[] CreaExcelMagazziniNegativi(MagazzinoDS ds)
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

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Negativi" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("IDMAGAZZ", CellValues.String, 2),
                    ConstructCell("Modello", CellValues.String, 2),
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
