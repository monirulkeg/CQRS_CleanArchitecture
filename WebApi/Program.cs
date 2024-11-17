
using Application;
using Application.ProductOperations.Commands;
using Application.ProductOperations.Queries;
using Infrastructure;
using MediatR;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthorization();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.MapGet("/products", async (ISender mediatr) =>
            {
                var products = await mediatr.Send(new ListProductsQuery());
                return Results.Ok(products);
            });

            app.MapPost("/products", async (CreateProductCommand command, ISender mediatr) =>
            {
                var productId = await mediatr.Send(command);
                if (Guid.Empty == productId) return Results.BadRequest();

                //var product = await mediatr.Send(new GetProductQuery(productId));
                //var productEntity = new Product()
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Description = product.Description,
                //    Price = product.Price
                //};
                //await mediatr.Publish(new ProductCreatedEvent(productEntity));

                return Results.Created($"/products/{productId}", new { id = productId });
            });

            app.Run();
        }
    }
}
