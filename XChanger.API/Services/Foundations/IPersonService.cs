using XChanger.API.Models.Data;
using XChanger.API.Models.DTOs;

namespace XChanger.API.Services.Foundations;

public interface IPersonService
{
    Task AddPersonsAsync(List<ExternalPerson> externalPersons);
    IQueryable<Person> RetrieveAllPersons();
}