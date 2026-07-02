using Backend.Adapter.Converter;
using Backend.UseCase.Handler;
using Backend.UseCase.Handler.Converter;
using Backend.UseCase.Interactor;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IInteractor, Interactor>();
builder.Services.AddScoped<IHandler, Handler>();

builder.Services.AddScoped<IResponsemodelConverter, ResponsemodelConverter>();
builder.Services.AddScoped<IProductConverter, ProductConverter>();

builder.Services.AddScoped<IRepository, Repository>();

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