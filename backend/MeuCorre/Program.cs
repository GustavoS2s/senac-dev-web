
using MeuCorre.Application;
using MeuCorre.Domain.interfaces.Repositories;
using MeuCorre.Infra;
using MeuCorre.Infra.Repositories;

namespace MeuCorre
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddAplication(builder.Configuration);
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeuCorre v1");
                c.RoutePrefix = "swagger"; // acessa em http://localhost:5283/swagger
            });
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
