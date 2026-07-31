using System.Globalization;
using Client.Helpers;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Models;

namespace Client.Services;

public class HttpProductApi(IHttpRequestExecuter executer) : IHttpProductApi
{
    public async Task<QueryResult<List<ProductDto>>> LoadProducts(ProductSearchConfiguration? filter = null)
            => await QueryWrapper(
                async () =>
                {
                    var result = await executer.ExecuteGetRequests<List<ProductDto>>(BuildUrl(ApiRoutes.Product.Base, filter));
                    return result.IsSuccess ? result.Data : null;
                },
                ErrorCodes.FailedConnection);

    public async Task<QueryResult<ProductDto>> LoadProduct(ProductId id)
        => await QueryWrapper(
            async () =>
            {
                var result = await executer.ExecuteGetRequests<ProductDto>(ApiRoutes.Product.ById(id.Value));
                return result.IsSuccess ? result.Data : null;
            },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<List<Category>>> LoadCategories()
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<List<Category>>(ApiRoutes.Category.Base);
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<Category>> LoadCategory(Category category)
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<Category>(ApiRoutes.Category.ById(category.Id.Value));
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<List<Description>>> LoadDescriptions()
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<List<Description>>(ApiRoutes.Description.Base);
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<Description>> LoadDescription(DescriptionId id)
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<Description>(ApiRoutes.Description.ById(id.Value));
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<Result> UpdateProduct(ProductDto product)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePutRequest<ProductDto>(ApiRoutes.Product.Base, product);
                    return result.IsSuccess;
                },
            ErrorCodes.DataUpdateFailed);

    public async Task<Result> UpdateCategory(Category category)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePutRequest<Category>(ApiRoutes.Category.Base, category);
                    return result.IsSuccess;
                },
            ErrorCodes.DataUpdateFailed);

    public async Task<Result> AddProduct(ProductDto product)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePostRequest<ProductDto>(ApiRoutes.Product.Base, product);
                    return result.IsSuccess;
                },
            ErrorCodes.DataCreationFailed);

    public async Task<Result> AddCategory(Category category)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePostRequest<Category>(ApiRoutes.Category.Base, category);
                    return result.IsSuccess;
                },
            ErrorCodes.DataCreationFailed);

    public async Task<Result> DeleteProduct(ProductId product)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecuteDeleteRequest<ProductId>(ApiRoutes.Product.Base, product);
                    return result.IsSuccess;
                },
            ErrorCodes.DataDeletionFailed);

    public async Task<Result> DeleteCategory(CategoryId category)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecuteDeleteRequest<CategoryId>(ApiRoutes.Category.Base, category);
                    return result.IsSuccess;
                },
            ErrorCodes.DataDeletionFailed);

    private async Task<QueryResult<T>> QueryWrapper<T>(Func<Task<T?>> queryOperation, ErrorCodes errorCode)
        where T : class
    {
        try
        {
            var data = await queryOperation();
            if (data != null)
            {
                return QueryResult<T>.Success(data);
            }
        }
        catch (Exception)
        {
            return QueryResult<T>.Failure(errorCode);
        }

        return QueryResult<T>.Failure(errorCode);
    }

    private async Task<Result> OperationWrapper(Func<Task<bool>> operation, ErrorCodes errorCode)
    {
        try
        {
            var isSuccess = await operation();
            if (isSuccess)
            {
                return Result.Success();
            }
        }
        catch (Exception)
        {
            return Result.Failure(ErrorCodes.NetworkError);
        }

        return Result.Failure(errorCode);
    }

    private static string BuildUrl(string baseUrl, ProductSearchConfiguration? filter)
    {
        if (filter == null)
        {
            return baseUrl;
        }

        var queryParams = new Dictionary<string, string?>
        {
            { "SearchTitle", filter.SearchTitle.ToString() },
            { "SearchShortDescription", filter.SearchShortDescription.ToString() },
            { "SearchLongDescription", filter.SearchLongDescription.ToString() },
            { "SearchByCategory", filter.SearchByCategory.ToString() },
            { "SearchByPrice", filter.SearchByPrice.ToString() },
            { "SearchByWeight", filter.SearchByWeight.ToString() },
        };

        if (!string.IsNullOrEmpty(filter.SearchText))
        {
            queryParams.Add("SearchText", filter.SearchText);
        }

        if (filter.MinPrice.HasValue)
        {
            queryParams.Add("MinPrice", filter.MinPrice.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (filter.MaxPrice.HasValue)
        {
            queryParams.Add("MaxPrice", filter.MaxPrice.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (filter.MinWeight.HasValue)
        {
            queryParams.Add("MinWeight", filter.MinWeight.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (filter.MaxWeight.HasValue)
        {
            queryParams.Add("MaxWeight", filter.MaxWeight.Value.ToString(CultureInfo.InvariantCulture));
        }

        var url = QueryHelpers.AddQueryString(baseUrl, queryParams);

        if (filter.SelectedCategoryIds != null)
        {
            foreach (var id in filter.SelectedCategoryIds)
            {
                url = QueryHelpers.AddQueryString(url, "SelectedCategoryIds", id.Value.ToString());
            }
        }

        return url;
    }
}