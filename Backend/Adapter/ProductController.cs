using Backend.UseCase.Interactor;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Backend.Adapter;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private IInteractor _interactor;

    public ProductController(IInteractor interactor)
    {
        _interactor = interactor;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var result = _interactor.GetAllProducts();

        if (result.Count <= 0)
        {
            return NotFound();
        }

        return Ok(result);
    }

    //[HttpGet]
    //public async Task<ActionResult<List<Product>>> GetProducts(Product product)
    //{
    //    // ToDo: Logik einfügen
    //}

    [HttpPost]
    public async void CreateElement(Entity.Product product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
    }

    [HttpPut]
    public async void UpdateElement(Entity.Product product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
    }

    [HttpDelete]
    public async void DeleteElement(Entity.Product product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
    }
}
