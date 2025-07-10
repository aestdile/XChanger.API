using XChanger.API.DataAccess.Repositories;
using XChanger.API.Models.Data;
using XChanger.API.Models.DTOs;

namespace XChanger.API.Services.Foundations;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task AddPersonsAsync(List<ExternalPerson> externalPersons)
    {
        foreach (var externalPerson in externalPersons)
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = externalPerson.PersonName,
                Age = externalPerson.Age,
                Pets = new List<Pet>()
            };

            AddPetIfValid(person, externalPerson.PetOne, externalPerson.PetOneType);
            AddPetIfValid(person, externalPerson.PetTwo, externalPerson.PetTwoType);
            AddPetIfValid(person, externalPerson.PetThree, externalPerson.PetThreeType);

            if (!string.IsNullOrEmpty(person.Name))
            {
                await _personRepository.InsertPersonAsync(person);
            }
        }
    }

    public IQueryable<Person> RetrieveAllPersons()
    {
        return _personRepository.SelectAllPersons();
    }

    private void AddPetIfValid(Person person, string petName, string petType)
    {
        if (!string.IsNullOrEmpty(petName) && petName != "-")
        {
            person.Pets.Add(new Pet
            {
                Id = Guid.NewGuid(),
                Name = petName,
                Type = ParsePetType(petType),
                PersonId = person.Id
            });
        }
    }

    private PetType ParsePetType(string petType)
    {
        return (PetType)Enum.Parse(typeof(PetType), petType, true);
    }
}