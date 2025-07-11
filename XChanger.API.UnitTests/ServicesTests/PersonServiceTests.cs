using Moq;
using XChanger.API.DataAccess.Repositories;
using XChanger.API.Models.Data;
using XChanger.API.Models.DTOs;
using XChanger.API.Services.Foundations;

namespace XChanger.API.UnitTests.ServicesTests
{
    public class PersonServiceUnitTests
    {
        private readonly Mock<IPersonRepository> _repoMock;
        private readonly PersonService _personService;

        public PersonServiceUnitTests()
        {
            _repoMock = new Mock<IPersonRepository>();
            _personService = new PersonService(_repoMock.Object);
        }

        #region Helper
        private void SetupCapture(out List<Person> captured)
        {
            var capturedList = new List<Person>();
            captured = capturedList;

            _repoMock.Setup(r => r.InsertPersonAsync(It.IsAny<Person>()))
                     .ReturnsAsync((Person p) => p)
                     .Callback<Person>(p => capturedList.Add(p));
        }
        #endregion

        [Fact(DisplayName = "Adds person with multiple valid pets")]
        public async Task AddPersonsAsync_AddsPerson_WithMultiplePets()
        {
            var external = new ExternalPerson
            {
                PersonName = "Mary",
                Age = 32,
                PetOne = "Markiza",
                PetOneType = "Cat",
                PetTwo = "Pushistik",
                PetTwoType = "Dog",
                PetThree = "Belka",
                PetThreeType = "Cat"
            };
            SetupCapture(out var stored);

            await _personService.AddPersonsAsync(new() { external });

            Assert.Single(stored);
            var person = stored.First();
            Assert.Equal(3, person.Pets.Count);
            Assert.Contains(person.Pets, p => p.Name == "Pushistik" && p.Type == PetType.Dog);
        }

        [Fact(DisplayName = "Skips minus-only pets and stores rest")]
        public async Task AddPersonsAsync_SkipsMinusPets()
        {
            var external = new ExternalPerson
            {
                PersonName = "Gary",
                Age = 18,
                PetOne = "Norm",
                PetOneType = "Dog",
                PetTwo = "-",
                PetTwoType = "-",
                PetThree = "-",
                PetThreeType = "-"
            };
            SetupCapture(out var stored);

            await _personService.AddPersonsAsync(new() { external });

            Assert.Single(stored);
            Assert.Single(stored.First().Pets); 
        }

        [Fact(DisplayName = "Skips person when name empty")]
        public async Task AddPersonsAsync_DoesNotAdd_WhenNameEmpty()
        {
            var external = new ExternalPerson
            {
                PersonName = "-",
                Age = 20,
                PetOne = "Tommy",
                PetOneType = "Cat"
            };
            await _personService.AddPersonsAsync(new() { external });
            _repoMock.Verify(r => r.InsertPersonAsync(It.IsAny<Person>()), Times.Once);
        }

        [Theory(DisplayName = "Parses pet type case‑insensitively")]
        [InlineData("Cat", PetType.Cat)]
        [InlineData("dog", PetType.Dog)]
        [InlineData("PARROT", PetType.Parrot)]
        public async Task AddPersonsAsync_ParsesPetType(string type, PetType expected)
        {
            var external = new ExternalPerson
            {
                PersonName = "Case",
                Age = 22,
                PetOne = "Buddy",
                PetOneType = type
            };
            SetupCapture(out var stored);
            await _personService.AddPersonsAsync(new() { external });
            Assert.Equal(expected, stored.First().Pets.First().Type);
        }

        [Fact(DisplayName = "Handles multiple rows with mixed data")]
        public async Task AddPersonsAsync_Handles_MixedList()
        {
            var externals = new List<ExternalPerson>
            {
                new()
                {
                    PersonName = "Alex", Age = 30,
                    PetOne = "Casper", PetOneType = "Cat",
                    PetTwo = "Rex",    PetTwoType = "Dog"
                },
                new()
                {
                    PersonName = "Mila", Age = 28,
                    PetOne = "-",       PetOneType = "-",
                    PetTwo = "Kesha",   PetTwoType = "Parrot"
                },
                new()
                {
                    PersonName = "Empty", Age = 50,
                    PetOne = "-", PetOneType = "-",
                    PetTwo = "-", PetTwoType = "-",
                    PetThree = "-", PetThreeType = "-"
                }
            };
            SetupCapture(out var stored);
            await _personService.AddPersonsAsync(externals);

            Assert.Equal(3, stored.Count);
            Assert.Equal(2, stored.First(p => p.Name == "Alex").Pets.Count);
            Assert.Single(stored.First(p => p.Name == "Mila").Pets);
            Assert.Empty(stored.First(p => p.Name == "Empty").Pets);
        }

        [Fact(DisplayName = "RetrieveAllPersons delegates to repository")]
        public void RetrieveAllPersons_CallsRepo()
        {
            var people = new List<Person> { new() { Name = "Any" } }.AsQueryable();
            _repoMock.Setup(r => r.SelectAllPersons()).Returns(people);
            var result = _personService.RetrieveAllPersons();
            Assert.Equal(people, result);
            _repoMock.Verify(r => r.SelectAllPersons(), Times.Once);
        }
    }
}
