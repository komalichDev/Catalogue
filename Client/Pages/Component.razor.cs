using System.Net.Http.Json;

namespace Client.Pages;

public partial class Component
{
    private List<Shared.Models.ProductDto>? _produktListe;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _produktListe = await Http.GetFromJsonAsync<List<Shared.Models.ProductDto>>("https://localhost:7053/api/Product");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            _produktListe = new List<Shared.Models.ProductDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.Message}");
            _produktListe = new List<Shared.Models.ProductDto>();
        }
    }
}
