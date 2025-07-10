using XChanger.API.Models.Data;

namespace XChanger.API.Services.Orchestrations;

public interface IOrchestrationService
{
    Task ProcessExcelData();
    IQueryable<Person> RetrieveAllPersonsWithPets();
}