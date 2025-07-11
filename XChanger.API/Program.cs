using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using XChanger.API.DataAccess;
using XChanger.API.DIContainer;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("XChangerDB")));

builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
builder.Services.AddProjectServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
