using System.Net.Http.Json;
using Common.Exception;
using Common.Types;

namespace Client.Pages;

public partial class Component
{
    private QueryResult<List<Shared.Models.ProductDto>>? _produktListe;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _produktListe = await Http.GetFromJsonAsync<QueryResult<List<Shared.Models.ProductDto>>>("https://localhost:7053/api/Product");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            _produktListe = QueryResult<List<Shared.Models.ProductDto>>.Failure(ErrorCodes.NetworkError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            _produktListe = QueryResult<List<Shared.Models.ProductDto>>.Failure(ErrorCodes.NetworkError);
        }
    }
}
