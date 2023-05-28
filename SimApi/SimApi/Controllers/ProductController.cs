using Microsoft.AspNetCore.Mvc;
using SimApi.Base;
using SimApi.Operation;
using SimApi.Schema;

namespace SimApi.Service.Controllers;

[Route("simapi/v1/[Controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService service;

    public ProductController(IProductService service)
    {
        this.service = service;
    }

    [HttpGet]
    public ApiResponse<IEnumerable<ProductResponse>> GetAll()
    {
        return service.GetAll();
    }

    [HttpGet("{id}")]
    public ApiResponse<ProductResponse> GetById(int id)
    {
        return service.GetById(id);
    }

    [HttpPost]
    public ApiResponse Post([FromBody] ProductRequest request)
    {
        return service.Insert(request);
    }

    [HttpPut("{id}")]
    public ApiResponse Put(int id, [FromBody] ProductRequest request)
    {
        return service.Update(id, request);
    }

    [HttpDelete("{id}")]
    public ApiResponse Delete(int id)
    {
        return service.Delete(id);
    }
}
