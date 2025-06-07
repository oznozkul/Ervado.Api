using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Services;
using Ervado.Infrastructure.Context;
using Ervado.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ervado.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ErvadoContext>());

            return services;
        }
    }
}
