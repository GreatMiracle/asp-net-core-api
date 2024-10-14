using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace WebApplication1.Core.Utils
{
    public class ExcelExportHelper
    {
        private ExcelPackage _package;
        private ExcelWorksheet _worksheet;

        public ExcelExportHelper(string sheetName)
        {
            _package = new ExcelPackage();
            _worksheet = _package.Workbook.Worksheets.Add(sheetName);
        }
        public void AddHeader(string[] headers, Color backgroundColor, Color textColor, FontStyle fontStyle)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = _worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(backgroundColor);
                cell.Style.Font.Color.SetColor(textColor);
                cell.Style.Font.Bold = (fontStyle & FontStyle.Bold) == FontStyle.Bold;
                cell.Style.Font.Italic = (fontStyle & FontStyle.Italic) == FontStyle.Italic;
                cell.Style.Font.Size = 12; // Bạn có thể thay đổi kích thước phông chữ nếu cần

                // Định dạng viền
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
        }

        public void AddRow(object[] values, int rowIndex)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var cell = _worksheet.Cells[rowIndex, i + 1];
                cell.Value = values[i];
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
        }

        public MemoryStream GetExcelStream()
        {
            var stream = new MemoryStream();
            _package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
