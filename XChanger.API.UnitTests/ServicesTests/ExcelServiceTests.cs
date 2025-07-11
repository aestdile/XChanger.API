using OfficeOpenXml;
using XChanger.API.Services.Foundations;

namespace XChanger.API.UnitTests.Services
{
    public class ExcelServiceUnitTests
    {
        private readonly ExcelService _excelService;

        public ExcelServiceUnitTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelService = new ExcelService();
        }

        private string CreateTempExcelFile(Action<ExcelWorksheet> populate)
        {
            var tempFile = Path.GetTempFileName() + ".xlsx";
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Sheet1");

            sheet.Cells[1, 1].Value = "PersonName";
            sheet.Cells[1, 2].Value = "Age";
            sheet.Cells[1, 3].Value = "Pet1";
            sheet.Cells[1, 4].Value = "Pet1 Type";
            sheet.Cells[1, 5].Value = "Pet2";
            sheet.Cells[1, 6].Value = "Pet2 Type";
            sheet.Cells[1, 7].Value = "Pet3";
            sheet.Cells[1, 8].Value = "Pet3 Type";

            populate(sheet);
            package.SaveAs(new FileInfo(tempFile));
            return tempFile;
        }

        [Fact(DisplayName = "Reads single row correctly")]
        public void ReadPersonsFromExcel_SingleRow()
        {
            string path = CreateTempExcelFile(ws =>
            {
                ws.Cells[2, 1].Value = "Mary";
                ws.Cells[2, 2].Value = 32;
                ws.Cells[2, 3].Value = "Markiza";
                ws.Cells[2, 4].Value = "Cat";
            });

            var people = _excelService.ReadPersonsFromExcel(path);
            Assert.Single(people);
            Assert.Equal("Mary", people[0].PersonName);
            Assert.Equal(32, people[0].Age);
            Assert.Equal("Markiza", people[0].PetOne);
        }

        [Fact(DisplayName = "Handles minus pet values")]
        public void ReadPersonsFromExcel_MinusPets()
        {
            string path = CreateTempExcelFile(ws =>
            {
                ws.Cells[2, 1].Value = "Gary";
                ws.Cells[2, 2].Value = 18;
                ws.Cells[2, 3].Value = "Norm";
                ws.Cells[2, 4].Value = "Dog";
                ws.Cells[2, 5].Value = "-";
                ws.Cells[2, 6].Value = "-";
            });

            var list = _excelService.ReadPersonsFromExcel(path);
            Assert.Single(list);
            var p = list[0];
            Assert.Equal("Gary", p.PersonName);
            Assert.Equal("-", p.PetTwo);
        }

        [Fact(DisplayName = "Reads multiple rows of mixed data")]
        public void ReadPersonsFromExcel_MultipleRows()
        {
            string path = CreateTempExcelFile(ws =>
            {
                ws.Cells[2, 1].Value = "Alex";
                ws.Cells[2, 2].Value = 25;
                ws.Cells[2, 3].Value = "Casper";
                ws.Cells[2, 4].Value = "Cat";

                ws.Cells[3, 1].Value = "Mila";
                ws.Cells[3, 2].Value = 28;
                ws.Cells[3, 3].Value = "-";
                ws.Cells[3, 4].Value = "-";
                ws.Cells[3, 5].Value = "Kesha";
                ws.Cells[3, 6].Value = "Parrot";
            });

            var list = _excelService.ReadPersonsFromExcel(path);
            Assert.Equal(2, list.Count);
            Assert.Equal("Alex", list[0].PersonName);
            Assert.Equal("Mila", list[1].PersonName);
        }

        [Fact(DisplayName = "Returns empty list when no data rows")]
        public void ReadPersonsFromExcel_EmptyFile()
        {
            string path = CreateTempExcelFile(ws => { /* no data rows */ });
            var result = _excelService.ReadPersonsFromExcel(path);
            Assert.Empty(result);
        }
    }
}
