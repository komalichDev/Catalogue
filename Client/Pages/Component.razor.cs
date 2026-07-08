using System.Net.Http.Json;
using Client.Helpers;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class Component
{
    private QueryResult<List<ProductDto>>? _produktListe;
    private string _errorMessage = string.Empty;
    private ProductDto? _selectedProduct = null;

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnInitializedAsync()
        => _produktListe = await HttpRequestExecuter.ExecuteGetRequests<List<ProductDto>>(Http, "https://localhost:7053/api/Product");

    private void DeleteProduct(Shared.Models.ProductDto product)
    {
    }

    private void ShowDetailedInfo(Shared.Models.ProductDto product) 
        => _selectedProduct = product;
}
