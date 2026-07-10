using System.ComponentModel.Design.Serialization;
using Common.Exception;
using Common.Types;
using DatabaseAccess.Context;
using DatabaseAccess.Converter;
using DatabaseAccess.Entity;
using DatabaseAccess.RepositoryModel;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;

public class ProductDatabaseAccess(IProductDbContext context) : IProductDatabaseAccess
{
    private readonly IProductDbContext _context = context;

    public async Task<QueryResult<RepositoryModel.ProductRepositoryModel>> GetAllProducts()
            => await QueryWrapper(
                () => _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Description)
                        .ToListAsync(),
                entities => ProductRepositoryModelConverter.Convert(entities),
                ErrorCodes.FailedConnection,
                "Fehler beim Abrufen der Produkte"
            );

    public async Task<QueryResult<List<RepositoryModel.Category>>> GetAllCategories() 
        => await QueryWrapper(
                    () => _context.Categories
                            .ToListAsync(),
                    entities => ProductRepositoryModelConverter.Convert(entities),
                    ErrorCodes.FailedConnection,
                    "Fehler beim Abrufen der Produkte"
        );

    public async Task<QueryResult<RepositoryModel.Product>> GetProduct(ProductId id)
        => await QueryWrapper(
            () => _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Description)
                    .FirstOrDefaultAsync(p => p.Id == id),
            entity => ProductRepositoryModelConverter.Convert(entity),
            ErrorCodes.FailedConnection,
            "Fehler beim Abrufen des Produkts"
        );

    public async Task<Result> CreateProduct(RepositoryModel.Product product)
            => await OperationWrapper(
                (productEntity) =>
                {
                    productEntity.Category = null;
                    productEntity.Description = null;
                    _context.Products.Add(productEntity);
                },
                ProductRepositoryModelConverter.Convert(product),
                ErrorCodes.DataCreationFailed,
                "Fehler beim Erstellen des Produkts: "
                );

    public async Task<Result> CreateCategory(RepositoryModel.Category category)
        => await OperationWrapper(
            (categoryEntity) => _context.Categories.Add(categoryEntity),
            ProductRepositoryModelConverter.Convert(category),
            ErrorCodes.DataCreationFailed,
            "Fehler beim Erstellen des Produkts: "
            );

    public async Task<Result> CreateDescription(RepositoryModel.Description description) 
        => await OperationWrapper(
            (descriptionEntity) => _context.Descriptions.Add(descriptionEntity),
            ProductRepositoryModelConverter.Convert(description),
            ErrorCodes.DataCreationFailed,
            "Fehler beim Erstellen des Produkts: "
            );

    public async Task<Result> DeleteProduct(ProductId id)
        => await OperationWrapper(
            (productEntity) => _context.Products.Remove(productEntity),
            await _context.Products.FirstOrDefaultAsync(p => p.Id == id),
            ErrorCodes.DataUpdateFailed,
            "Fehler beim Löschen des Produkts:"
            );

    public async Task<Result> DeleteCategory(CategoryId id)
        => await OperationWrapper(
            (categoryEntity) => _context.Categories.Remove(categoryEntity),
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == id),
            ErrorCodes.DataUpdateFailed,
            "Fehler beim Löschen des Produkts:"
            );

    public async Task<Result> DeleteDescription(DescriptionId id) 
        => await OperationWrapper(
            (descriptionEntity) => _context.Descriptions.Remove(descriptionEntity), 
            await _context.Descriptions.FirstOrDefaultAsync(d => d.Id == id), 
            ErrorCodes.DataUpdateFailed, 
            "Fehler beim Aktualisieren des Produkts: "
            );

    public async Task<Result> UpdateProduct(RepositoryModel.Product product)
            => await OperationWrapper(
                (productEntity) =>
                {
                    var updatedEntity = ProductRepositoryModelConverter.Convert(product);
                    _context.Entry(productEntity).CurrentValues.SetValues(updatedEntity);
                },
                await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id),
                ErrorCodes.DataUpdateFailed,
                "Fehler beim Aktualisieren des Produkts: "
                );

    public async Task<Result> UpdateCategory(RepositoryModel.Category category)
        => await OperationWrapper(
            (categoryEntity) =>
            {
                var updatedEntity = ProductRepositoryModelConverter.Convert(category);
                _context.Entry(categoryEntity).CurrentValues.SetValues(updatedEntity);
            },
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id),
            ErrorCodes.DataUpdateFailed,
            "Fehler beim Aktualisieren der Kategorie: "
            );

    public async Task<Result> UpdateDescription(RepositoryModel.Description description)
        => await OperationWrapper(
            (descriptionEntity) =>
            {
                var updatedEntity = ProductRepositoryModelConverter.Convert(description);
                _context.Entry(descriptionEntity).CurrentValues.SetValues(updatedEntity);
            },
            await _context.Descriptions.FirstOrDefaultAsync(d => d.Id == description.Id),
            ErrorCodes.DataUpdateFailed,
            "Fehler beim Aktualisieren der Beschreibung: "
            );

    public async Task<QueryResult<RepositoryModel.Category>> GetCategory(CategoryId id)
        => await QueryWrapper(
            () => _context.Categories
                    .FirstOrDefaultAsync(p => p.Id == id),
            entity => ProductRepositoryModelConverter.Convert(entity),
            ErrorCodes.FailedConnection,
            "Fehler beim Abrufen des Produkts"
        );

    public async Task<QueryResult<List<RepositoryModel.Description>>> GetAllDescriptions()
        => await QueryWrapper(
                    () => _context.Descriptions
                            .ToListAsync(),
                    entities => ProductRepositoryModelConverter.Convert(entities),
                    ErrorCodes.FailedConnection,
                    "Fehler beim Abrufen der Produkte"
        );

    public async Task<QueryResult<RepositoryModel.Description>> GetDescription(DescriptionId id)
        => await QueryWrapper(
            () => _context.Descriptions
                    .FirstOrDefaultAsync(p => p.Id == id),
            entity => ProductRepositoryModelConverter.Convert(entity),
            ErrorCodes.FailedConnection,
            "Fehler beim Abrufen des Produkts"
        );

    private async Task<Result> OperationWrapper<T>(
        Action<T> operation, 
        T? entity, 
        ErrorCodes code, 
        string errorMessage)
    {
        try
        {
            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }
            operation(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(errorMessage + $"{ex.Message}");
            return Result.Failure(code);
        }
    }

    private static async Task<QueryResult<TResult>> QueryWrapper<TEntity, TResult>(
            Func<Task<TEntity?>> queryOperation,
            Func<TEntity, TResult> converter,
            ErrorCodes errorCode,
            string errorMessage)
        where TResult : class
    {
        try {
            var entity = await queryOperation();
            if (entity == null) {
                return (QueryResult<TResult>)Result.Failure(errorCode);
            }
            return QueryResult<TResult>.Success(converter(entity));
        } catch (Exception ex) {
            Console.WriteLine($"{errorMessage}: {ex.Message}");
            return (QueryResult<TResult>)Result.Failure(errorCode);
        }
    }

}