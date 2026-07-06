using Backend.Repository;
using Backend.UseCase.Interactor;
using DatabaseAccess;
using DatabaseAccess.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IInteractor, Interactor>();
builder.Services.AddScoped<IProductGateway, ProductRepository>();

builder.Services.AddScoped<IProductDatabaseAccess, ProductDatabaseAccess>();
string connectionString = "Server=127.0.0.1;Port=3307;Database=product;Uid=root;Pwd=1234;";
builder.Services.AddDbContext<IProductDbContext, ProductDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7180", "http://localhost:5121", "https://localhost:7053")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers();

app.Run();