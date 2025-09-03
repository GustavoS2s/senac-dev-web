using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Infra
{
    public static  class Dependencyinjection
    {
       public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Mysql");
              
            services.AddDbContext<Data.Context.MeuDBBContext>(options =>
            options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

            return services;
        }
    }
}
