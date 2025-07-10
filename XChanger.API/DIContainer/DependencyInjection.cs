using XChanger.API.DataAccess.Repositories;
using XChanger.API.Services.Foundations;
using XChanger.API.Services.Orchestrations;

namespace XChanger.API.DIContainer;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddTransient<IPersonRepository, PersonRepository>();

        services.AddTransient<IExcelService, ExcelService>();
        services.AddTransient<IPersonService, PersonService>();
        services.AddTransient<IOrchestrationService, OrchestrationService>();

        return services;
    }
}