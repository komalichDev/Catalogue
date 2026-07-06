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
        var productsList = new ProductRepositoryModel();
        productsList.Products = new List<Repositorymodel.Product>();

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
