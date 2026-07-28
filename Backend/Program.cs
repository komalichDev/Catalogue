using Backend.Repository;
using Backend.UseCase.Interactor;
using Common.Config;
using DatabaseAccess;
using DatabaseAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

MapsterConfig.RegisterMappings();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.MapType<Common.Types.ProductId>(() => new OpenApiSchema { Type = JsonSchemaType.Integer, Format = "int32" });
    options.MapType<Common.Types.CategoryId>(() => new OpenApiSchema { Type = JsonSchemaType.Integer, Format = "int32" });
    options.MapType<Common.Types.DescriptionId>(() => new OpenApiSchema { Type = JsonSchemaType.Integer, Format = "int32" });
});

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers();

app.Run();