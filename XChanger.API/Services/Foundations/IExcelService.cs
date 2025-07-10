using XChanger.API.Models.DTOs;

namespace XChanger.API.Services.Foundations;

public interface IExcelService
{
    List<ExternalPerson> ReadPersonsFromExcel(string filePath);
}