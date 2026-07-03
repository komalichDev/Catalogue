using Backend.Repository;
using Backend.UseCase.Interactor;
using DatabaseAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IInteractor, Interactor>();
builder.Services.AddScoped<IProductGateway, ProductRepository>();

builder.Services.AddScoped<IProductDatabaseAccess, ProductDatabaseAccess>();

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