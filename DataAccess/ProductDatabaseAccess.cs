using Common.Exception;
using Common.Types;
using DatabaseAccess.Context;
using DatabaseAccess.Converter;
using DatabaseAccess.Entity;
using DatabaseAccess.Repositorymodel;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;

public class ProductDatabaseAccess(IProductDbContext context) : IProductDatabaseAccess
{
    private readonly IProductDbContext _context = context;

    public async Task<QueryResult<Repositorymodel.ProductRepositoryModel>> GetAllProducts()
    {
        try {
            var entities = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Description)
                            .ToListAsync();

            return QueryResult<Repositorymodel.ProductRepositoryModel>.Success(ProductRepositoryModelConverter.Convert(entities));
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Abrufen der Produkte: {ex.Message}");
            return QueryResult<Repositorymodel.ProductRepositoryModel>.Failure(ErrorCodes.FailedConnection);
        }
    }

    public async Task<QueryResult<Repositorymodel.Product>> GetProduct(ProductId id)
    {
        try {
            var entity = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Description)
                            .FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) {
                return QueryResult<Repositorymodel.Product>.Failure(ErrorCodes.NotFound);
            }
            return QueryResult<Repositorymodel.Product>.Success(ProductRepositoryModelConverter.Convert(entity));
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Abrufen des Produkts: {ex.Message}");
            return QueryResult<Repositorymodel.Product>.Failure(ErrorCodes.FailedConnection);
        }
    }

    public async Task<Result> CreateProduct(Repositorymodel.Product product)
    {
        try {
            var entity = ProductRepositoryModelConverter.Convert(product);
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Erstellen des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }
    }

    public async Task<Result> CreateCategory(Repositorymodel.Category category)
    {
        try {
            var entity = ProductRepositoryModelConverter.Convert(category);
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Erstellen des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }
    }

    public async Task<Result> CreateDescription(Repositorymodel.Description description)
    {
        try {
            var entity = ProductRepositoryModelConverter.Convert(description);
            _context.Descriptions.Add(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Erstellen des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }
    }

    public async Task<Result> DeleteProduct(ProductId id)
    {
        try {
            var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }

            _context.Products.Remove(entity);

            await _context.SaveChangesAsync();

            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Löschen des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataDeletionFailed);
        }
    }

    public async Task<Result> DeleteCategory(CategoryId id)
    {
        try {
            var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }

            _context.Categories.Remove(entity);

            await _context.SaveChangesAsync();

            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Löschen des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataDeletionFailed);
        }
    }

    public async Task<Result> DeleteDescription(DescriptionId id)
    {
        try {
            var entity = await _context.Descriptions.FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }

            _context.Descriptions.Remove(entity);

            await _context.SaveChangesAsync();

            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Löschen des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataDeletionFailed);
        }
    }

    public async Task<Result> UpdateProduct(Repositorymodel.Product product)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

        try {
            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }

            entity = ProductRepositoryModelConverter.Convert(product);

            await _context.SaveChangesAsync();
            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Aktualisieren des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataUpdateFailed);
        }


    }

    public async Task<Result> UpdateCategory(Repositorymodel.Category category)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        try {
            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }

            entity = ProductRepositoryModelConverter.Convert(category);

            await _context.SaveChangesAsync();
            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Aktualisieren des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataUpdateFailed);
        }
    }

    public async Task<Result> UpdateDescription(Repositorymodel.Description description)
    {
        var entity = await _context.Descriptions.FirstOrDefaultAsync(d => d.Id == description.Id);

        try {
            if (entity == null) {
                return Result.Failure(ErrorCodes.NotFound);
            }

            entity = ProductRepositoryModelConverter.Convert(description);

            await _context.SaveChangesAsync();
            return Result.Success();
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Aktualisieren des Produkts: {ex.Message}");
            return Result.Failure(ErrorCodes.DataUpdateFailed);
        }
    }
}
