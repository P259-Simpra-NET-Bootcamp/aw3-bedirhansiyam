using AutoMapper;
using SimApi.Base;
using SimApi.Data;
using SimApi.Data.Uow;
using SimApi.Schema;
using static Dapper.SqlMapper;

namespace SimApi.Operation;

public class ProductService : BaseService<Product, ProductRequest, ProductResponse>, IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public override ApiResponse Insert(ProductRequest request)
    {
        var exist = unitOfWork.Repository<Product>().Where(x => x.Name.Equals(request.Name)).ToList();
        if(exist.Any())
        {
            return new ApiResponse($"The product named '{request.Name}' already exists");
        }

        return base.Insert(request);
    }

    public override ApiResponse<IEnumerable<ProductResponse>> GetAll()
    {
        try
        {
            var entityList = unitOfWork.Repository<Product>().GetAll(x => x.Category);
            var mapped = mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(entityList);
            return new ApiResponse<IEnumerable<ProductResponse>>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ProductResponse>>(ex.Message);
        }
    }

    public override ApiResponse<ProductResponse> GetById(int id)
    {
        try
        {
            var entity = unitOfWork.Repository<Product>().GetById(id, x => x.Category);
            if (entity is null)
            {
                return new ApiResponse<ProductResponse>("Product not found");
            }

            var mapped = mapper.Map<Product, ProductResponse>(entity);
            return new ApiResponse<ProductResponse>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductResponse>(ex.Message);
        }
    }
}
