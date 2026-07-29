using BlazorBootstrap;
using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class ProductEditor(IHttpProductApi productApi) : BaseComponent
{
    private readonly IHttpProductApi productApi = productApi;
    private Modal categoryModal = default!;
    private string editProductName = string.Empty;
    private double editProductPrice = 0.0;

    private CategoryId editCategoryId;
    private string editShortSummary = string.Empty;
    private string editDetailedText = string.Empty;
    private int editWeight = 0;
    private CategorySelecter categorySelecter = default!;
    private bool isLoading = false;

    private ProductDto? product;

    [Parameter]
    public ProductId Id { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (Id.Value == 0)
        {
            this.product = new ProductDto(ProductId.From(0), string.Empty, 0.0, DescriptionId.From(0), null, CategoryId.From(0), null);
            this.editProductName = string.Empty;
            this.editProductPrice = 0.0;

            this.editCategoryId = CategoryId.From(0);
            this.editShortSummary = string.Empty;
            this.editDetailedText = string.Empty;
            this.editWeight = 0;

            this.isLoading = false;
        }
        else
        {
            this.isLoading = true;
            await this.LoadData(this.Id);
            this.isLoading = false;
        }
    }

    private async Task<bool> SaveData(ProductDto dto)
        => await this.ExecuteActionAsync(() =>
        dto.Id == 0 ? this.productApi.AddProduct(dto) : this.productApi.UpdateProduct(dto));

    private async Task OpenCategoryEditorModal()
        => await this.categoryModal.ShowAsync<CategoryEditor>(title: "Kategorien verwalten");

    private async Task CancelEdit()
        => await this.OnClose.InvokeAsync();

    private async Task LoadData(ProductId id)
    {
        var product = await this.ExecuteLoadAsync(() => this.productApi.LoadProduct(id));

        if (product != null)
        {
            this.product = product;
            this.editProductName = this.product.Name;
            this.editProductPrice = this.product.Price;
            this.editCategoryId = this.product.CategoryId;

            if (this.product.Description != null)
            {
                this.editShortSummary = this.product.Description.ShortSummary;
                this.editDetailedText = this.product.Description.DetailedText;
                this.editWeight = this.product.Description.WeightInGrams;
            }
        }
    }

    private async Task HandleSave()
    {
        if (this.product != null)
        {
            var newDescription = new Shared.Models.Description(
                this.product.DescriptionId,
                this.editShortSummary,
                this.editDetailedText,
                this.editWeight);

            var updatedProduct = this.product with
            {
                Name = this.editProductName,
                Price = this.editProductPrice,
                CategoryId = this.editCategoryId,
                Category = null,
                Description = newDescription,
            };

            bool isSuccess = await this.SaveData(updatedProduct);

            if (isSuccess)
            {
                await this.OnClose.InvokeAsync();
            }
        }
    }

    private void HandleCategoryChanged(CategoryId newId)
        => this.editCategoryId = newId;

    private async Task OnCategoryModalClosed()
    {
        if (this.categorySelecter != null)
        {
            await this.categorySelecter.LoadData();
        }
    }
}
