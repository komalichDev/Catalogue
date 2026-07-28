using Common.Types;
using Shared.Models;

namespace Client.Services;

public interface IHttpProductApi
{
    Task<QueryResult<List<ProductDto>>> LoadProducts();

    Task<QueryResult<ProductDto>> LoadProduct(ProductId id);

    Task<QueryResult<List<Category>>> LoadCategories();

    Task<QueryResult<Category>> LoadCategory(Category category);

    Task<QueryResult<List<Description>>> LoadDescriptions();

    Task<QueryResult<Description>> LoadDescription(DescriptionId id);

    Task<Result> UpdateProduct(ProductDto product);

    Task<Result> UpdateCategory(Category category);

    Task<Result> AddProduct(ProductDto product);

    Task<Result> AddCategory(Category category);

    Task<Result> DeleteProduct(ProductId product);

    Task<Result> DeleteCategory(CategoryId category);
}