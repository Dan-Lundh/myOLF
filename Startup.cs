using Microsoft.Extensions.DependencyInjection;
using Blazored.Modal;


public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add Blazored.Modal
        services.AddBlazoredModal();
    }

    public void Configure()
    {
        // Configure other services and middleware if needed
    }
}