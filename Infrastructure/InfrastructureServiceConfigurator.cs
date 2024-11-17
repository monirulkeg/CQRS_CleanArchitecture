using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Infrastructure;

public static class InfrastructureServiceConfigurator
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Debug.WriteLine($"Connection string: {connectionString}");

        //Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
            //options.UseInMemoryDatabase("SchoolAppData");
        });
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        //services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}
