using XChanger.API.Models.Data;
using XChanger.API.Services.Foundations;

namespace XChanger.API.Services.Orchestrations;

public class OrchestrationService : IOrchestrationService
{
    private readonly IExcelService _excelService;
    private readonly IPersonService _personService;

    public OrchestrationService(IExcelService excelService, IPersonService personService)
    {
        _excelService = excelService;
        _personService = personService;
    }

    public async Task ProcessExcelData()
    {
        var externalPersons = _excelService.ReadPersonsFromExcel("Storage/Files/data.xlsx");
        await _personService.AddPersonsAsync(externalPersons);
    }

    public IQueryable<Person> RetrieveAllPersonsWithPets()
    {
        return _personService.RetrieveAllPersons();
    }
}