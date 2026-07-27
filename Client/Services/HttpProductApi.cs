using Client.Helpers;
using Common.Exception;
using Common.Types;
using Shared.Models;

namespace Client.Services;

public class HttpProductApi(IHttpRequestExecuter executer) : IHttpProductApi
{
    public async Task<QueryResult<List<ProductDto>>> LoadProducts()
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<List<ProductDto>>($"https://localhost:7053/api/Product");
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<ProductDto>> LoadProduct(ProductId id)
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<ProductDto>($"https://localhost:7053/api/Product/Product/{id.Value}");
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<List<Category>>> LoadCategories()
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<List<Category>>($"https://localhost:7053/api/Product/Category/");
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<Category>> LoadCategory(Category category)
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<Category>($"https://localhost:7053/api/Product/Category/{category.Id.Value}");
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<List<Description>>> LoadDescriptions()
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<List<Description>>($"https://localhost:7053/api/Product/Description/");
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<QueryResult<Description>> LoadDescription(DescriptionId id)
        => await QueryWrapper(
            async () =>
                {
                    var result = await executer.ExecuteGetRequests<Description>($"https://localhost:7053/api/Product/Description/{id.Value}");
                    return result.IsSuccess ? result.Data : null;
                },
            ErrorCodes.FailedConnection);

    public async Task<Result> UpdateProduct(ProductDto product)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePutRequest<ProductDto>($"https://localhost:7053/api/Product/{product.Id.Value}", product);
                    return result.IsSuccess;
                },
            ErrorCodes.DataUpdateFailed);

    public async Task<Result> UpdateCategory(Category category)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePutRequest<Category>($"https://localhost:7053/api/Product/Category/{category.Id.Value}", category);
                    return result.IsSuccess;
                },
            ErrorCodes.DataUpdateFailed);

    public async Task<Result> AddProduct(ProductDto product)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePostRequest<ProductDto>($"https://localhost:7053/api/Product/", product);
                    return result.IsSuccess;
                },
            ErrorCodes.DataCreationFailed);

    public async Task<Result> AddCategory(Category category)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecutePostRequest<Category>($"https://localhost:7053/api/Product/Category/", category);
                    return result.IsSuccess;
                },
            ErrorCodes.DataCreationFailed);

    public async Task<Result> DeleteProduct(ProductId product)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecuteDeleteRequest<ProductId>($"https://localhost:7053/api/Product/{product.Value}", product);
                    return result.IsSuccess;
                },
            ErrorCodes.DataDeletionFailed);

    public async Task<Result> DeleteCategory(CategoryId category)
        => await OperationWrapper(
            async () =>
                {
                    var result = await executer.ExecuteDeleteRequest<CategoryId>($"https://localhost:7053/api/Product/Category/{category.Value}", category);
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
}