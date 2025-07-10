using XChanger.API.Models.Data;

namespace XChanger.API.DataAccess.Repositories;

public interface IPersonRepository
{
    Task<Person> InsertPersonAsync(Person person);
    IQueryable<Person> SelectAllPersons();
}