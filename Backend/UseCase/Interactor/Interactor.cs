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
        var category = await GetCategoryOrNullAsync(product.CategoryId);
        if (category == null)
        {
            return Result.Failure(ErrorCodes.CategoryNotFound);
        }

        var prepResult = await ValidateAndCreateDescriptionAsync(product);
        if (!prepResult.IsSuccess)
        {
            return prepResult;
        }

        var addedDescription = await RetrieveNewlyCreatedDescriptionAsync(product.Description);
        if (product.Description != null && addedDescription == null)
        {
            return Result.Failure(ErrorCodes.DescriptionCreationFailed);
        }

        var newProduct = BuildProductEntity(product, addedDescription, category);

        return await SaveProductWithRollbackAsync(newProduct, addedDescription?.Id);
    }

    public async Task<Result> CreateCategory(Category category)
            => (await _gateway.GetCategoryById(category.Id)).IsSuccess
                ? Result.Success()
                : await ExecuteSequentialAsync(
                    async () => await IdenticalDataPresent(ProductDtoConverter.Convert(category)) ? Result.Failure(ErrorCodes.CategoryAlreadyExists) : Result.Success(),
                    () => ExecuteIfNotNullAsync(category, c => _gateway.CreateCategory(ProductDtoConverter.Convert(c)), ErrorCodes.CategoryCreationFailed));

    public async Task<Result> UpdateProduct(ProductDto product)
            => await ExecuteSequentialAsync(
                () => ExecuteIfNotNullAsync(product?.Category, c => _gateway.UpdateCategory(ProductDtoConverter.Convert(c)), ErrorCodes.CategoryUpdateFailed),
                () => ExecuteIfNotNullAsync(product?.Description, d => _gateway.UpdateDescription(ProductDtoConverter.Convert(d)), ErrorCodes.DescriptionUpdateFailed),
                () => ExecuteIfNotNullAsync(product, p => _gateway.UpdateProduct(ProductDtoConverter.Convert(p)), ErrorCodes.ProductUpdateFailed));

    public async Task<Result> UpdateCategory(Category category)
            => await ExecuteIfNotNullAsync(category, c => _gateway.UpdateCategory(ProductDtoConverter.Convert(c)), ErrorCodes.CategoryUpdateFailed);

    public async Task<Result> DeleteProduct(ProductId productId)
    {
        var product = (await _gateway.GetProductById(productId)).Data;

        return await ExecuteSequentialAsync(
            () => ExecuteActionAsync(() => _gateway.DeleteProduct(productId), ErrorCodes.ProductDeletionFailed),
            () => ExecuteIfNotNullAsync(product?.Description, d => _gateway.DeleteDescription(ProductDtoConverter.Convert(d).Id), ErrorCodes.DescriptionDeletionFailed));
    }

    public async Task<Result> DeleteCategory(CategoryId category)
            => await IsCategoryInUse(category)
                ? Result.Failure(ErrorCodes.CategoryInUse)
                : await ExecuteActionAsync(() => _gateway.DeleteCategory(category), ErrorCodes.CategoryDeletionFailed);

    private async Task<bool> IsCategoryInUse(CategoryId id)
    {
        var products = await _gateway.GetAllProducts();
        return products.IsSuccess && products.Data?.Any(product => product.CategoryId == id) == true;
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
            return QueryResult<TResult>.Failure(result.ErrorCode);
        }

        var data = result.Data ?? fallbackData;

        return data != null
            ? QueryResult<TResult>.Success(converter(data))
            : QueryResult<TResult>.Failure(result.ErrorCode);
    }

    private async Task<Entity.Category?> GetCategoryOrNullAsync(CategoryId categoryId)
    {
        var categoryCheck = await _gateway.GetCategoryById(categoryId);
        return categoryCheck.IsSuccess ? categoryCheck.Data : null;
    }

    private async Task<Result> ValidateAndCreateDescriptionAsync(ProductDto product)
    {
        var convertedProduct = ProductDtoConverter.Convert(product);
        return await ExecuteSequentialAsync(
            async () => await IdenticalDataPresent(convertedProduct) ? Result.Failure(ErrorCodes.ProductAlreadyExists) : Result.Success(),
            async () => await IdenticalDataPresent(convertedProduct.Description) ? Result.Failure(ErrorCodes.DescriptionAlreadyExists) : Result.Success(),
            () => ExecuteIfNotNullAsync(product.Description, d => _gateway.CreateDescription(ProductDtoConverter.Convert(d)), ErrorCodes.DescriptionCreationFailed));
    }

    private Entity.Product BuildProductEntity(ProductDto product, Description? addedDescription, Entity.Category category)
    {
        var descId = addedDescription != null ? DescriptionId.From(addedDescription.Id.Value) : DescriptionId.From(0);
        var convertedDesc = addedDescription != null ? ProductDtoConverter.Convert(addedDescription) : null;

        return new Entity.Product(
            product.Id,
            product.Name,
            product.Price,
            descId,
            convertedDesc!,
            CategoryId.From(product.CategoryId.Value),
            new Entity.Category(CategoryId.From(category.Id.Value), category.Name));
    }

    private async Task<Result> SaveProductWithRollbackAsync(Entity.Product newProduct, DescriptionId? createdDescriptionId)
    {
        var productResult = await _gateway.CreateProduct(newProduct);
        if (productResult.IsSuccess)
        {
            return Result.Success();
        }

        if (createdDescriptionId != null)
        {
            var rollback = await _gateway.DeleteDescription(createdDescriptionId.Value);
            if (!rollback.IsSuccess)
            {
                return Result.Failure(ErrorCodes.DataDeletionAndCreationOfProductFailded);
            }
        }

        return Result.Failure(ErrorCodes.ProductCreationFailed);
    }

    private async Task<Result> ExecuteSequentialAsync(params Func<Task<Result>>[] operations)
    {
        foreach (var operation in operations)
        {
            var result = await operation();
            if (!result.IsSuccess)
            {
                return result;
            }
        }

        return Result.Success();
    }

    private async Task<Result> ExecuteIfNotNullAsync<T>(T? data, Func<T, Task<Result>> action, ErrorCodes failureCode) where T : class
    {
        if (data == null)
        {
            return Result.Success();
        }

        var result = await action(data);
        return result.IsSuccess ? Result.Success() : Result.Failure(failureCode);
    }

    private async Task<Result> ExecuteActionAsync(Func<Task<Result>> action, ErrorCodes failureCode)
    {
        var result = await action();
        return result.IsSuccess ? Result.Success() : Result.Failure(failureCode);
    }

    private async Task<Description?> RetrieveNewlyCreatedDescriptionAsync(Shared.Models.Description? originalDescription)
    {
        if (originalDescription == null)
        {
            return null;
        }

        var descriptions = await GetAllDescriptions();
        if (descriptions.Data == null)
        {
            return null;
        }

        return descriptions.Data
            .OrderBy(c => c.Id.Value)
            .FirstOrDefault(desc =>
                desc.DetailedText == originalDescription.DetailedText &&
                desc.ShortSummary == originalDescription.ShortSummary &&
                desc.WeightInGrams == originalDescription.WeightInGrams)
            ?? originalDescription;
    }

    private async Task<bool> IdenticalDataPresent(Entity.Product givenProduct)
            => await CheckIdenticalDataAsync(
                () => _gateway.GetAllProducts(),
                product => product.Name == givenProduct.Name && product.Price == givenProduct.Price);

    private async Task<bool> IdenticalDataPresent(Entity.Category givenCategory)
        => await CheckIdenticalDataAsync(
            () => _gateway.GetAllCategories(),
            category => category.Name == givenCategory.Name);

    private async Task<bool> IdenticalDataPresent(Entity.Description givenDesc)
        => await CheckIdenticalDataAsync(
            () => _gateway.GetAllDescriptions(),
            desc => desc.WeightInGrams == givenDesc.WeightInGrams &&
                    desc.DetailedText == givenDesc.DetailedText &&
                    desc.ShortSummary == givenDesc.ShortSummary);

    private async Task<bool> CheckIdenticalDataAsync<T>(
        Func<Task<QueryResult<List<T>>>> getListFunc,
        Func<T, bool> identicalCondition)
        where T : class
    {
        var results = await getListFunc();

        return !results.IsSuccess ||
               results.Data == null ||
               results.Data.Any(identicalCondition);
    }
}
