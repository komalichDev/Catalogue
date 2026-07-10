using Backend.UseCase.Interactor.Converter;
using Common.Exception;
using Common.Types;
using Shared.Models;

namespace Backend.UseCase.Interactor;

public class Interactor : IInteractor
{
    private IProductGateway _gateway;

    public Interactor(IProductGateway gateway)
        => _gateway = gateway;

    public async Task<QueryResult<List<ProductDto>>> GetAllProducts()
            => await ExecuteQueryAsync(
                () => _gateway.GetAllProducts(),
                data => ProductDtoConverter.Convert(data),
                new List<Entity.Product>());

    public async Task<QueryResult<ProductDto>> GetProductById(ProductId id)
        => await ExecuteQueryAsync(
            () => _gateway.GetProductById(id),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<Description>> GetDescriptionById(DescriptionId id)
        => await ExecuteQueryAsync(
            () => _gateway.GetDescriptionById(id),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<Category>> GetCategoryById(CategoryId id)
        => await ExecuteQueryAsync(
            () => _gateway.GetCategoryById(id),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<List<Category>>> GetAllCategories()
        => await ExecuteQueryAsync(
            () => _gateway.GetAllCategories(),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<List<Description>>> GetAllDescriptions()
        => await ExecuteQueryAsync(
            () => _gateway.GetAllDescriptions(),
            data => ProductDtoConverter.Convert(data));

    public async Task<Result> CreateProduct(ProductDto product)
    {
        // ToDo: Refactoring
        if (product.Category == null)
        {
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }

        var convertedProduct = ProductDtoConverter.Convert(product);

        if (await IdenticalDataPresent(convertedProduct))
        {
            return Result.Failure(ErrorCodes.IdenticalData);
        }

        if (await IdenticalDataPresent(convertedProduct.Description))
        {
            return Result.Failure(ErrorCodes.IdenticalData);
        }

        if (product.Description != null)
        {
            var descriptionResult = await _gateway.CreateDescription(ProductDtoConverter.Convert(product.Description));
            if (!descriptionResult.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataCreationFailed);
            }
        }

        var descriptions = GetAllDescriptions();

        if (descriptions.Result.Data == null)
        {
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }

        List<Description> sortedDescriptions = (List<Description>)(descriptions.Result.Data.OrderBy(c => c.Id));

        if (product.Description == null)
        {
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }

        var addedDescription = GetIdOfNewlyCreatedDescription(sortedDescriptions, product.Description);
        var newProduct = new Entity.Product(
                product.Id,
                product.Name,
                product.Price,
                DescriptionId.From(addedDescription.Id.Value),
                ProductDtoConverter.Convert(addedDescription),
                CategoryId.From(product.CategoryId.Value),
                new Entity.Category(CategoryId.From(product.CategoryId.Value), string.Empty));

        var productResult = await _gateway.CreateProduct(newProduct);
        if (!productResult.IsSuccess)
        {
            return Result.Failure(ErrorCodes.DataCreationFailed);
        }

        return Result.Success();
    }

    public async Task<Result> CreateCategory(Category category)
    {
        var result = await _gateway.GetCategoryById(category.Id);
        if (!result.IsSuccess)
        {
            if (await IdenticalDataPresent(ProductDtoConverter.Convert(category)))
            {
                return Result.Failure(ErrorCodes.IdenticalData);
            }

            if (category != null)
            {
                var categoryResult = await _gateway.CreateCategory(ProductDtoConverter.Convert(category));
                if (!categoryResult.IsSuccess)
                {
                    return Result.Failure(ErrorCodes.DataCreationFailed);
                }
            }
        }

        return Result.Success();
    }

    public async Task<Result> UpdateProduct(ProductDto product)
    {
        if (product.Category != null)
        {
            var categoryResult = await _gateway.UpdateCategory(ProductDtoConverter.Convert(product.Category));
            if (!categoryResult.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataUpdateFailed);
            }
        }

        if (product.Description != null)
        {
            var descriptionResult = await _gateway.UpdateDescription(ProductDtoConverter.Convert(product.Description));
            if (!descriptionResult.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataUpdateFailed);
            }
        }

        if (product != null)
        {
            var productResult = await _gateway.UpdateProduct(ProductDtoConverter.Convert(product));
            if (!productResult.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataUpdateFailed);
            }
        }

        return Result.Success();
    }

    public async Task<Result> UpdateCategory(Category category)
    {
        if (category != null)
        {
            var categoryResult = await _gateway.UpdateCategory(ProductDtoConverter.Convert(category));
            if (!categoryResult.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataUpdateFailed);
            }
        }

        return Result.Success();
    }

    public async Task<Result> DeleteProduct(ProductDto product)
    {
        var result = await _gateway.DeleteProduct(ProductDtoConverter.Convert(product).Id);
        if (!result.IsSuccess)
        {
            return Result.Failure(ErrorCodes.DataDeletionFailed);
        }

        if (product.Description != null)
        {
            var id = ProductDtoConverter.Convert(product.Description).Id;
            var resultDescription = await _gateway.DeleteDescription(id);
            if (!resultDescription.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataDeletionFailed);
            }
        }

        return Result.Success();
    }

    public async Task<Result> DeleteCategory(CategoryId category)
    {
        var result = await _gateway.DeleteCategory(category);
        if (!result.IsSuccess)
        {
            return Result.Failure(ErrorCodes.DataDeletionFailed);
        }

        return Result.Success();
    }

    private async Task<QueryResult<TResult>> ExecuteQueryAsync<TData, TResult>(
        Func<Task<QueryResult<TData>>> gatewayCall,
        Func<TData, TResult> converter,
        TData? fallbackData = null)
        where TResult : class
        where TData : class
    {
        var result = await gatewayCall();

        if (!result.IsSuccess)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(result.ErrorCode);
        }

        var data = result.Data ?? fallbackData;
        if (data == null)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(result.ErrorCode);
        }

        return QueryResult<TResult>.Success(converter(data));
    }

    private Description GetIdOfNewlyCreatedDescription(List<Description> sortedDescriptionsById, Description givenDescription)
    {
        foreach (var description in sortedDescriptionsById)
        {
            if (description.DetailedText == givenDescription.DetailedText && description.ShortSummary == givenDescription.ShortSummary && description.WeightInGrams == givenDescription.WeightInGrams)
            {
                return description;
            }
        }

        return givenDescription;
    }

    private Category GetIdOfNewlyCreatedCategory(List<Category> sortedCategoriesById, Category givenCategory)
    {
        foreach (var category in sortedCategoriesById)
        {
            if (category.Name == givenCategory.Name)
            {
                return category;
            }
        }

        return givenCategory;
    }

    private async Task<bool> IdenticalDataPresent(Entity.Product givenProduct)
    {
        var results = await _gateway.GetAllProducts();
        if (!results.IsSuccess || results.Data == null)
        {
            return true;
        }

        var products = results.Data;
        foreach (var product in products)
        {
            if (givenProduct.Name == product.Name && givenProduct.Price == product.Price)
            {
                return true;
            }
        }

        return false;
    }

    private async Task<bool> IdenticalDataPresent(Entity.Category givenCategory)
    {
        var results = await _gateway.GetAllCategories();
        if (!results.IsSuccess || results.Data == null)
        {
            return true;
        }

        var categories = results.Data;
        foreach (var catagory in categories)
        {
            if (givenCategory.Name == catagory.Name)
            {
                return true;
            }
        }

        return false;
    }

    private async Task<bool> IdenticalDataPresent(Entity.Description givenDescription)
    {
        var results = await _gateway.GetAllDescriptions();
        if (!results.IsSuccess || results.Data == null)
        {
            return true;
        }

        var descriptions = results.Data;
        foreach (var category in descriptions)
        {
            if (givenDescription.WeightInGrams == category.WeightInGrams && givenDescription.DetailedText == category.DetailedText && givenDescription.ShortSummary == category.ShortSummary)
            {
                return true;
            }
        }

        return false;
    }
}
