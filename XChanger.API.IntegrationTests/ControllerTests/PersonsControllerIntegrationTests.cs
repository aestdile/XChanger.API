using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using OfficeOpenXml;

namespace XChanger.API.IntegrationTests
{
    public class PersonsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PersonsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/persons returns 200 and JSON body")]
        public async Task GetAllPersons_ReturnsOkJson()
        {
            var response = await _client.GetAsync("/api/persons");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("name", json, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "GET /api/persons/pets returns 200 and JSON array")]
        public async Task GetAllPets_ReturnsOkJson()
        {
            var response = await _client.GetAsync("/api/persons/pets");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact(DisplayName = "GET /api/persons/with-pets returns 200 and XML body")]
        public async Task GetAllPersonsWithPets_ReturnsXml()
        {
            var response = await _client.GetAsync("/api/persons/with-pets");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/xml", response.Content.Headers.ContentType?.MediaType);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<?xml", content);
            Assert.Contains("<ArrayOfPerson", content);
        }

        [Fact(DisplayName = "POST /api/persons/process-excel processes data and returns message")]
        public async Task ProcessExcel_ReturnsSuccessMessage()
        {
            EnsureTestExcelFile();           

            var response = await _client.PostAsync("/api/persons/process-excel", null);
            var text = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Data processed", text);
        }

        private void EnsureTestExcelFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string source = Path.Combine("..", "..", "..", "..", "XChanger.API", "Storage", "Files", "data.xlsx");
            string targetDir = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Files");
            Directory.CreateDirectory(targetDir);
            File.Copy(source, Path.Combine(targetDir, "data.xlsx"), overwrite: true);
        }

    }
}