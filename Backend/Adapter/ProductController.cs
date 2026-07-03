using Backend.Adapter.Converter;

using Backend.UseCase.Interactor;

using Backend.UseCase.Interactor.Requestmodel;

using Backend.UseCase.Interactor.Requests;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Adapter;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private IInteractor _interactor;

    private IProductConverter _productConverter;

    public ProductController(IInteractor interactor, IProductConverter productConverter)
    {
        _interactor = interactor;
        _productConverter = productConverter;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var data = default(Requestmodel);
        var result = _interactor.Execute(Requests.GetAllElements, data);

        if (result.Products.Count <= 0)
        {
            return NotFound();
        }

        return Ok(_productConverter.Convert(result));
    }

    //[HttpGet]
    //public async Task<ActionResult<List<Product>>> GetProducts(Product product)
    //{
    //    // ToDo: Logik einfügen
    //}

    [HttpPost]
    public async void CreateElement(Product product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
    }

    [HttpPut]
    public async void UpdateElement(Product product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
    }

    [HttpDelete]
    public async void DeleteElement(Product product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
    }
}
