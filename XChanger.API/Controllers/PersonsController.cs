using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using XChanger.API.Services.Orchestrations;

namespace XChanger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IOrchestrationService _orchestrationService;

    public PersonsController(IOrchestrationService orchestrationService)
    {
        _orchestrationService = orchestrationService;
    }

    [HttpPost("process-excel")]
    public async Task<IActionResult> ProcessExcel()
    {
        try
        {
            await _orchestrationService.ProcessExcelData();
            return Ok("Data processed and stored successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Error: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult GetAllPersons()
    {
        var persons = _orchestrationService.RetrieveAllPersonsWithPets()
            .Select(p => new { p.Id, p.Name, p.Age });
        return Ok(persons);
    }

    [HttpGet("pets")]
    public IActionResult GetAllPets()
    {
        var pets = _orchestrationService.RetrieveAllPersonsWithPets().SelectMany(p => p.Pets);
        return Ok(pets);
    }

    [HttpGet("with-pets")]
    public IActionResult GetAllPersonsWithPets()
    {
        var persons = _orchestrationService.RetrieveAllPersonsWithPets().ToList();

        var serializer = new XmlSerializer(persons.GetType());
        var stringBuilder = new StringBuilder();

        using (var stringWriter = new StringWriter(stringBuilder))
        {
            serializer.Serialize(stringWriter, persons);
        }

        return Content(stringBuilder.ToString(), "application/xml");
    }
}