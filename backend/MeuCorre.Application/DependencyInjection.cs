using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeuCorre.Application
{
    public static class Dependencyinjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
