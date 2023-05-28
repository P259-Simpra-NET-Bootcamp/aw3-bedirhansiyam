using AutoMapper;
using SimApi.Base;
using SimApi.Data;
using SimApi.Data.Uow;
using SimApi.Schema;
using static Dapper.SqlMapper;

namespace SimApi.Operation;

public class CategoryService : BaseService<Category, CategoryRequest, CategoryResponse>, ICategoryService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }


    public override ApiResponse Insert(CategoryRequest request)
    {
        var exist = unitOfWork.Repository<Category>().Where(x => x.Name.Equals(request.Name)).ToList();
        if (exist.Any())
        {
            return new ApiResponse($"The category named '{request.Name}' already exists");
        }

        return base.Insert(request);
    }

    public override ApiResponse<IEnumerable<CategoryResponse>> GetAll()
    {
        try
        {
            var entityList = unitOfWork.Repository<Category>().GetAll(x => x.Products);
            var mapped = mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(entityList);
            return new ApiResponse<IEnumerable<CategoryResponse>>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<CategoryResponse>>(ex.Message);
        }
    }

    public override ApiResponse<CategoryResponse> GetById(int id)
    {
        try
        {
            var entity = unitOfWork.Repository<Category>().GetById(id, x => x.Products);
            if (entity is null)
            {
                return new ApiResponse<CategoryResponse>("Category not found");
            }

            var mapped = mapper.Map<Category, CategoryResponse>(entity);
            return new ApiResponse<CategoryResponse>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponse>(ex.Message);
        }
    }
}
