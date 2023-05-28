using Microsoft.AspNetCore.Mvc;
using SimApi.Base;
using SimApi.Operation;
using SimApi.Schema;

namespace SimApi.Service.Controllers;

[Route("simapi/v1/[Controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService service;

    public CategoryController(ICategoryService service)
    {
        this.service = service;
    }

    [HttpGet]
    public ApiResponse<IEnumerable<CategoryResponse>> GetAll()
    {
        return service.GetAll();
    }

    [HttpGet("{id}")]
    public ApiResponse<CategoryResponse> GetById(int id)
    {
        return service.GetById(id);
    }

    [HttpPost]
    public ApiResponse Post([FromBody] CategoryRequest request)
    {
        return service.Insert(request);
    }

    [HttpPut("{id}")]
    public ApiResponse Put(int id, [FromBody] CategoryRequest request)
    {
        return service.Update(id, request);
    }

    [HttpDelete("{id}")]
    public ApiResponse Delete(int id)
    {
        return service.Delete(id);
    }
}
