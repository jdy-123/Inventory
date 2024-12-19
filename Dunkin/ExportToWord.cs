using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Windows.Forms;

namespace Dunkin
{
    public partial class ExportToWord : Form
    {
        public ExportToWord()
        {
            InitializeComponent();
        }

        private void ExportToWord_Load(object sender, EventArgs e)
        {

        }

        // Function to export selected rows from DataGridView to Word document
        public void ExportSelectedRowsToWord(DataGridView dataGridView, string filePath)
        {
            // Create a new Word document using Open XML SDK
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Add a MainDocumentPart to the Word document
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());

                // Get the body of the document
                Body body = mainPart.Document.Body;

                // Create a table for the Word document
                Table table = new Table();

                // Define the table style with borders
                TableProperties tblProperties = new TableProperties(
                    new TableStyle() { Val = "TableGrid" }, // Using TableGrid for a default grid style
                    new TableWidth() { Type = TableWidthUnitValues.Auto },
                    new TableBorders(
                        new TopBorder() { Val = BorderValues.Single, Size = 4 },
                        new BottomBorder() { Val = BorderValues.Single, Size = 4 },
                        new LeftBorder() { Val = BorderValues.Single, Size = 4 },
                        new RightBorder() { Val = BorderValues.Single, Size = 4 },
                        new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 4 },
                        new InsideVerticalBorder() { Val = BorderValues.Single, Size = 4 }
                    )
                );
                table.Append(tblProperties);

                // Add header row to the table (excluding the "ID" column)
                TableRow headerRow = new TableRow();
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Skip the "ID" column
                    if (column.Name != "ID")
                    {
                        // Create header cell with text and add borders
                        TableCell headerCell = new TableCell(
                            new Paragraph(
                                new Run(new Text(column.HeaderText))
                            )
                        );
                        // Add style to header cell (bold text and borders)
                        headerCell.Append(new TableCellProperties(
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center },
                            new TableCellBorders(
                                new TopBorder() { Val = BorderValues.Single, Size = 4 },
                                new BottomBorder() { Val = BorderValues.Single, Size = 4 },
                                new LeftBorder() { Val = BorderValues.Single, Size = 4 },
                                new RightBorder() { Val = BorderValues.Single, Size = 4 }
                            )
                        ));
                        headerRow.Append(headerCell);
                    }
                }
                table.Append(headerRow);

                // Loop through selected rows in the DataGridView and add them to the table
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    TableRow tableRow = new TableRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Skip the "ID" column
                        if (cell.OwningColumn.Name != "ID")
                        {
                            // Create table cell with text and add borders
                            TableCell tableCell = new TableCell(
                                new Paragraph(
                                    new Run(new Text(cell.Value?.ToString() ?? string.Empty))
                                )
                            );

                            // Apply word wrapping and cell borders
                            tableCell.Append(new TableCellProperties(
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center },
                                new TableCellBorders(
                                    new TopBorder() { Val = BorderValues.Single, Size = 4 },
                                    new BottomBorder() { Val = BorderValues.Single, Size = 4 },
                                    new LeftBorder() { Val = BorderValues.Single, Size = 4 },
                                    new RightBorder() { Val = BorderValues.Single, Size = 4 }
                                ),
                                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" } // Fixed width for columns
                            ));

                            // Ensure that text wraps inside the cell by setting the word wrapping in the paragraph
                            var paragraph = tableCell.GetFirstChild<Paragraph>();
                            var paragraphProperties = paragraph.GetFirstChild<ParagraphProperties>() ?? new ParagraphProperties();
                            var justification = new Justification() { Val = JustificationValues.Both }; // Justify text

                            paragraphProperties.Append(justification);
                            paragraph.PrependChild(paragraphProperties);

                            // Add the cell to the row
                            tableRow.Append(tableCell);
                        }
                    }
                    table.Append(tableRow);
                }

                // Add the table to the document's body
                body.Append(table);
            }

            // Let the user know the file was created
            MessageBox.Show("Word document created at: " + filePath);
        }
    }
}
