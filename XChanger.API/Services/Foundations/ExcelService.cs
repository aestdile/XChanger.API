using OfficeOpenXml;
using XChanger.API.Models.DTOs;

namespace XChanger.API.Services.Foundations;

public class ExcelService : IExcelService
{
    public List<ExternalPerson> ReadPersonsFromExcel(string filePath)
    {
        var persons = new List<ExternalPerson>();
        var fileInfo = new FileInfo(filePath);

        using (var package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var person = new ExternalPerson
                {
                    PersonName = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty,
                    Age = int.Parse(worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? "0"),
                    PetOne = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty,
                    PetOneType = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty,
                    PetTwo = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? string.Empty,
                    PetTwoType = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty,
                    PetThree = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty,
                    PetThreeType = worksheet.Cells[row, 8].Value?.ToString()?.Trim() ?? string.Empty
                };
                persons.Add(person);
            }
        }

        return persons;
    }
}