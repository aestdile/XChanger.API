using Moq;
using XChanger.API.Models.Data;
using XChanger.API.Models.DTOs;
using XChanger.API.Services.Foundations;
using XChanger.API.Services.Orchestrations;

namespace XChanger.API.UnitTests.ServicesTests;

public class OrchestrationServiceTests
{
    private readonly Mock<IExcelService> _mockExcelService;
    private readonly Mock<IPersonService> _mockPersonService;
    private readonly OrchestrationService _orchestrationService;

    public OrchestrationServiceTests()
    {
        _mockExcelService = new Mock<IExcelService>();
        _mockPersonService = new Mock<IPersonService>();
        _orchestrationService = new OrchestrationService(_mockExcelService.Object, _mockPersonService.Object);
    }

    [Fact]
    public async Task ProcessExcelData_ShouldReadAndProcessData_WhenCalled()
    {
        var externalPersons = new List<ExternalPerson>
        {
            new ExternalPerson
            {
                PersonName = "Mary",
                Age = 32,
                PetOne = "Markiza",
                PetOneType = "Cat"
            }
        };

        _mockExcelService.Setup(x => x.ReadPersonsFromExcel("Storage/Files/data.xlsx"))
            .Returns(externalPersons);

        _mockPersonService.Setup(x => x.AddPersonsAsync(It.IsAny<List<ExternalPerson>>()))
            .Returns(Task.CompletedTask);

        await _orchestrationService.ProcessExcelData();

        _mockExcelService.Verify(x => x.ReadPersonsFromExcel("Storage/Files/data.xlsx"), Times.Once);
        _mockPersonService.Verify(x => x.AddPersonsAsync(externalPersons), Times.Once);
    }

    [Fact]
    public void RetrieveAllPersonsWithPets_ShouldReturnPersons_WhenCalled()
    {
        var persons = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Mary",
                Age = 32,
                Pets = new List<Pet>
                {
                    new Pet { Id = Guid.NewGuid(), Name = "Markiza", Type = PetType.Cat }
                }
            }
        }.AsQueryable();

        _mockPersonService.Setup(x => x.RetrieveAllPersons())
            .Returns(persons);

        var result = _orchestrationService.RetrieveAllPersonsWithPets();

        Assert.NotNull(result);
        Assert.Single(result);
        _mockPersonService.Verify(x => x.RetrieveAllPersons(), Times.Once);
    }

    [Fact]
    public async Task ProcessExcelData_ShouldHandleEmptyExcelData_WhenNoDataFound()
    {
        var emptyExternalPersons = new List<ExternalPerson>();

        _mockExcelService.Setup(x => x.ReadPersonsFromExcel("Storage/Files/data.xlsx"))
            .Returns(emptyExternalPersons);

        _mockPersonService.Setup(x => x.AddPersonsAsync(It.IsAny<List<ExternalPerson>>()))
            .Returns(Task.CompletedTask);

        await _orchestrationService.ProcessExcelData();

        _mockExcelService.Verify(x => x.ReadPersonsFromExcel("Storage/Files/data.xlsx"), Times.Once);
        _mockPersonService.Verify(x => x.AddPersonsAsync(emptyExternalPersons), Times.Once);
    }
}