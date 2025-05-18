using Ervado.Application.Common.Services;
using Ervado.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ervado.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        
        return services;
    }
} 