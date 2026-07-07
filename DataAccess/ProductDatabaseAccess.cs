using DatabaseAccess.Context;
using DatabaseAccess.Converter;
using DatabaseAccess.Repositorymodel;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;

public class ProductDatabaseAccess(IProductDbContext context) : IProductDatabaseAccess
{
    private readonly IProductDbContext _context = context;

    public async Task<Repositorymodel.ProductRepositoryModel> GetAllProducts()
    {
        try {
            var entities = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Description)
                            .ToListAsync();

            return ProductRepositoryModelConverter.Convert(entities);
        } catch (Exception ex) {
            Console.WriteLine($"Fehler beim Abrufen der Produkte: {ex.Message}");
            throw;
        }
    }
}
