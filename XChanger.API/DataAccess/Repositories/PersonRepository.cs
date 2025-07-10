using Microsoft.EntityFrameworkCore;
using XChanger.API.Models.Data;

namespace XChanger.API.DataAccess.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _context;

    public PersonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Person> InsertPersonAsync(Person person)
    {
        var result = await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public IQueryable<Person> SelectAllPersons()
    {
        return _context.Persons.Include(p => p.Pets);
    }
}