using MeuCorre.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuCorre.Infra
{
    public static  class Dependencyinjection
    {
       public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Mysql");
              
            services.AddDbContext<MeuDbContext>(options =>
            options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

            return services;
        }
    }
}
