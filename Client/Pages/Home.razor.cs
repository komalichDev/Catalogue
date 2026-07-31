using System.Text.Json;
using BlazorBootstrap;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Client.Pages;

public partial class Home
{
    private ProductDto? _newProduct;
    private Modal _productModal = default!;
    private Component _productComponent = default!;
    private Modal _categoryModal = default!;
    private Modal _rangeModal = default!;

    private ProductSearchConfiguration _currentSearchConfig = new();

    private async Task OpenProductEditorAsync()
    {
        _newProduct = new ProductDto(
                    Id: ProductId.From(0),
                    Name: string.Empty,
                    Price: 0.0,
                    DescriptionId: DescriptionId.From(0),
                    Description: new Description(DescriptionId.From(0), string.Empty, string.Empty, 0),
                    CategoryId: CategoryId.From(0),
                    Category: new Category (CategoryId.From(0), string.Empty));

        var parameters = new Dictionary<string, object>
        {
            { "Id", ProductId.From(0) },
            { "OnClose", EventCallback.Factory.Create(this, CloseEditor) },
        };

        await _productModal.ShowAsync<ProductEditor>(
            title: "Neues Produkt erstellen",
            parameters: parameters);
    }

    private async Task CloseEditor()
    {
        await _productModal.HideAsync();
        _newProduct = null;
        if (_productComponent != null)
        {
            await _productComponent.LoadData();
        }
    }

    private async Task HandleSearch(ProductSearchConfiguration config)
    {
        _currentSearchConfig.SearchText = config.SearchText;
        if (!string.IsNullOrEmpty(config.SearchText))
        {
            _currentSearchConfig.SearchTitle = true;
            _currentSearchConfig.SearchShortDescription = true;
            _currentSearchConfig.SearchLongDescription = true;
        }
        else
        {
            _currentSearchConfig.SearchTitle = false;
            _currentSearchConfig.SearchShortDescription = false;
            _currentSearchConfig.SearchLongDescription = false;
        }

        var jsonConfig = JsonSerializer.Serialize(_currentSearchConfig, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine($"--- Suche gestartet ---\n{jsonConfig}");
        if (_productComponent != null)
        {
            await _productComponent.ApplySearchAsync(_currentSearchConfig);
        }
    }

    private void ToggleCategorySearch()
    {
        _currentSearchConfig.SearchByCategory = !_currentSearchConfig.SearchByCategory;
    }

    private void TogglePriceSearch()
    {
        _currentSearchConfig.SearchByPrice = !_currentSearchConfig.SearchByPrice;

        if (_currentSearchConfig.SearchByPrice)
        {
            _currentSearchConfig.MinPrice ??= 0;
            _currentSearchConfig.MaxPrice ??= 100;
        }
    }

    private void ToggleWeightSearch()
    {
        _currentSearchConfig.SearchByWeight = !_currentSearchConfig.SearchByWeight;

        if (_currentSearchConfig.SearchByWeight)
        {
            _currentSearchConfig.MinWeight ??= 0;
            _currentSearchConfig.MaxWeight ??= 1000;
        }
    }

    private async Task OpenCategorySettings()
    {
        var parameters = new Dictionary<string, object>
        {
            { "SelectedCategoryIds", _currentSearchConfig.SelectedCategoryIds },
            {
                "OnConfigSaved", EventCallback.Factory.Create<List<CategoryId>>(this, async (ids) =>
                {
                _currentSearchConfig.SelectedCategoryIds = ids;
                _currentSearchConfig.SearchByCategory = ids != null && ids.Count > 0;
                await _categoryModal.HideAsync();
                })
            },
        };
        await _categoryModal.ShowAsync<CategorySearchSelection>(title: "Kategorie-Filter", parameters: parameters);
    }

    private async Task OpenPriceSettings()
    {
        var parameters = new Dictionary<string, object>
        {
            { "Title", "Preisbereich definieren" },
            { "Unit", "CHF" },
            { "DefaultMin", 0.0 },
            { "DefaultMax", 100.0 },
            {
                "OnConfigSaved", EventCallback.Factory.Create<(double?, double?)>(this, async (range) =>
                {
                    _currentSearchConfig.MinPrice = range.Item1;
                    _currentSearchConfig.MaxPrice = range.Item2;
                    _currentSearchConfig.SearchByPrice = range.Item1.HasValue || range.Item2.HasValue;
                    await _rangeModal.HideAsync();
                })
            },
        };

        if (_currentSearchConfig.MinPrice.HasValue)
        {
            parameters.Add("MinValue", _currentSearchConfig.MinPrice.Value);
        }

        if (_currentSearchConfig.MaxPrice.HasValue)
        {
            parameters.Add("MaxValue", _currentSearchConfig.MaxPrice.Value);
        }

        await _rangeModal.ShowAsync<RangeSearchSelection>(title: "Preis-Filter", parameters: parameters);
    }

    private async Task OpenWeightSettings()
    {
        var parameters = new Dictionary<string, object>
        {
            { "Title", "Gewichtsbereich definieren" },
            { "Unit", "Gramm (g)" },
            { "DefaultMin", 0.0 },
            { "DefaultMax", 1000.0 },
            {
                "OnConfigSaved", EventCallback.Factory.Create<(double?, double?)>(this, async (range) =>
                {
                    _currentSearchConfig.MinWeight = range.Item1;
                    _currentSearchConfig.MaxWeight = range.Item2;
                    _currentSearchConfig.SearchByWeight = range.Item1.HasValue || range.Item2.HasValue;
                    await _rangeModal.HideAsync();
                })
            },
        };

        if (_currentSearchConfig.MinWeight.HasValue)
        {
            parameters.Add("MinValue", _currentSearchConfig.MinWeight.Value);
        }

        if (_currentSearchConfig.MaxWeight.HasValue)
        {
            parameters.Add("MaxValue", _currentSearchConfig.MaxWeight.Value);
        }

        await _rangeModal.ShowAsync<RangeSearchSelection>(title: "Gewichts-Filter", parameters: parameters);
    }
}
